using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;
/// <summary>SQL Server persistence. JSON is used only as an import/export and SQL parameter format.</summary>
public static class AppDatabase
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true
    };
    public static string ConnectionString
    {
        get
        {
            var configured = Environment.GetEnvironmentVariable("SUONMUOI_CONNECTION_STRING") ?? Environment.GetEnvironmentVariable("LUAMOC_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured;
            }

            var path = Path.Combine(AppContext.BaseDirectory, "App.config");
            if (!File.Exists(path))
            {
                path = typeof(AppDatabase).Assembly.Location + ".config";
            }

            if (!File.Exists(path))
            {
                throw new InvalidOperationException("Không tìm thấy App.config chứa kết nối LuaMocDB.");
            }

            return XDocument.Load(path).Descendants("add")
            .FirstOrDefault(x => (string?)x.Attribute("name") == "LuaMocDB")?.Attribute("connectionString")?.Value ?? throw new InvalidOperationException("Chưa khai báo chuỗi kết nối LuaMocDB trong App.config.");
        }
    }

    public static string DatabaseName
    {
        get
        {
            return new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;
        }
    }

    public static string ServerName
    {
        get
        {
            return new SqlConnectionStringBuilder(ConnectionString).DataSource;
        }
    }

    private static SqlConnection Open()
    {
        var connection = new SqlConnection(ConnectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (SqlException e)
        {
            connection.Dispose();
            throw new InvalidOperationException(
                "Không kết nối được SQL Server. Kiểm tra dịch vụ SQL Server và kết nối LuaMocDB trong App.config.\n" + e.Message,
                e
            );
        }
    }

    private static SqlCommand Command(SqlConnection connection, SqlTransaction? transaction, string sql)
    {
        return new(sql, connection, transaction)
        {
            CommandTimeout = 60
        };
    }

    private static string Resource(string name)
    {
        using var stream = typeof(AppDatabase).Assembly.GetManifestResourceStream("Quan_Li_Nha_Hang_Suon_Muoi.Database." + name) ?? throw new InvalidOperationException("Thiếu tài nguyên SQL: " + name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static object? Scalar(SqlConnection c, SqlTransaction tx, string sql)
    {
        using var cmd = Command(c, tx, sql);
        return cmd.ExecuteScalar();
    }

    public static void Provision()
    {
        var builder = new SqlConnectionStringBuilder(ConnectionString);
        var name = builder.InitialCatalog;
        if (!Regex.IsMatch(name, @"\A[A-Za-z][A-Za-z0-9_]{0,63}\z"))
        {
            throw new InvalidOperationException("Tên DB không hợp lệ.");
        }

        builder.InitialCatalog = "master";
        using (var master = new SqlConnection(builder.ConnectionString))
        {
            master.Open();
            using var cmd = new SqlCommand($"IF DB_ID(@name) IS NULL CREATE DATABASE [{name}];", master)
            {
                CommandTimeout = 60
            };
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 128).Value = name;
            cmd.ExecuteNonQuery();
        }

        using var c = Open();
        foreach (var batch in Regex.Split(Resource("Schema.sql"), @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(batch))
            {
                using var cmd = Command(c, null, batch);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static RestaurantState Initialize(string directory, Func<RestaurantState> seed)
    {
        using var c = Open();
        using var tx = c.BeginTransaction(IsolationLevel.Serializable);
        using var gate = Command(c, tx, "SELECT Initialized FROM dbo.ApplicationState WITH (UPDLOCK,HOLDLOCK) WHERE Id=1;");
        var initialized = gate.ExecuteScalar() ?? throw new InvalidOperationException("DB chưa được tạo. Chạy Database/CreateDatabase.sql trong SSMS trước.");
        if (!(bool)initialized)
        {
            var path = Path.Combine(directory, "restaurant.json");
            var state = File.Exists(path) ? JsonSerializer.Deserialize<RestaurantState>(File.ReadAllText(path), Json) ?? throw new InvalidDataException("Không đọc được dữ liệu cũ.") : seed();
            if (state.Version != 1)
            {
                throw new InvalidDataException("Phiên bản dữ liệu cũ chưa được hỗ trợ.");
            }

            WriteState(c, tx, state);
            foreach (var key in CatalogSchema.Modules.Keys)
            {
                var catalogPath = Path.Combine(directory, "catalog-" + key + ".json");
                DataTable table;
                if (File.Exists(catalogPath))
                {
                    var saved = JsonSerializer.Deserialize<SavedCatalog>(File.ReadAllText(catalogPath)) ?? throw new InvalidDataException("Không đọc được danh mục cũ: " + key);
                    table = new DataTable(key);
                    foreach (var header in saved.Columns)
                    {
                        table.Columns.Add(header);
                    }

                    foreach (var row in saved.Rows)
                    {
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    table = DemoModuleData.CreateTable(key);
                }

                WriteCatalog(
                    c,
                    tx,
                    key,
                    table,
                    null
                );
            }

            SeedUser(
                c,
                tx,
                "admin",
                "Quản trị viên",
                "Admin",
                "admin123"
            );
            SeedUser(
                c,
                tx,
                "staff",
                "Nhân viên nhà hàng",
                "Staff",
                "staff123"
            );
        }

        var result = ReadState(c, tx);
        tx.Commit();
        return result;
    }

    private static RestaurantState ReadState(SqlConnection c, SqlTransaction tx)
    {
        // Acquire the coordinating row before reading any child table; readers and writers
        // take locks in the same order, avoiding a shared-lock/update-lock conversion cycle.
        Scalar(c, tx, "SELECT Revision FROM dbo.ApplicationState WITH(UPDLOCK,HOLDLOCK) WHERE Id=1;");
        using var cmd = Command(c, tx, Resource("ReadState.sql"));
        // FOR JSON may return several chunks for larger databases.
        using var reader = cmd.ExecuteReader();
        var text = new System.Text.StringBuilder();
        while (reader.Read())
        {
            text.Append(reader.GetString(0));
        }

        var state = JsonSerializer.Deserialize<RestaurantState>(text.ToString(), Json) ?? throw new InvalidDataException("Dữ liệu DB không hợp lệ.");
        if (state.Version != 1)
        {
            throw new InvalidDataException("Phiên bản dữ liệu SQL Server chưa được hỗ trợ.");
        }

        return state;
    }

    public static RestaurantState LoadState()
    {
        using var c = Open();
        using var tx = c.BeginTransaction(IsolationLevel.Serializable);
        var result = ReadState(c, tx);
        tx.Commit();
        return result;
    }

    private static void WriteState(SqlConnection c, SqlTransaction tx, RestaurantState state)
    {
        using var cmd = Command(c, tx, Resource("WriteState.sql"));
        cmd.Parameters.Add("@payload", SqlDbType.NVarChar, -1).Value = JsonSerializer.Serialize(state, Json);
        cmd.ExecuteNonQuery();
    }

    public static RestaurantState Change(long expectedRevision, Action<RestaurantState> action)
    {
        using var c = Open();
        using var tx = c.BeginTransaction(IsolationLevel.Serializable);
        var revision = Convert.ToInt64(Scalar(c, tx, "SELECT Revision FROM dbo.ApplicationState WITH(UPDLOCK,HOLDLOCK) WHERE Id=1;"));
        if (revision != expectedRevision)
        {
            throw new InvalidOperationException("Dữ liệu đã thay đổi ở cửa sổ khác. Hãy làm mới và thực hiện lại.");
        }

        var latest = ReadState(c, tx);
        action(latest);
        latest.Revision = revision + 1;
        WriteState(c, tx, latest);
        tx.Commit();
        return latest;
    }

    private static CatalogDefinition Definition(string key)
    {
        return CatalogSchema.Modules.TryGetValue(key, out var definition) ? definition : throw new InvalidOperationException("Danh mục không được hỗ trợ: " + key);
    }

    private static DataTable ReadCatalog(SqlConnection c, SqlTransaction tx, string key)
    {
        var definition = Definition(key);
        using var version = Command(c, tx, "SELECT Revision FROM dbo.CatalogRevisions WITH(UPDLOCK,HOLDLOCK) WHERE ModuleKey=@key;");
        version.Parameters.Add("@key", SqlDbType.NVarChar, 64).Value = key;
        var revision = version.ExecuteScalar() ?? throw new InvalidOperationException("Danh mục chưa được khởi tạo: " + key);
        var table = new DataTable(key);
        foreach (var col in definition.Columns)
        {
            table.Columns.Add(col.Header);
        }

        using var cmd = Command(
            c,
            tx,
            "SELECT " + string.Join(',', definition.Columns.Select(x => "[" + x.Name + "]")) + " FROM dbo.[" + definition.Table + "] ORDER BY [" + definition.Columns[0].Name + "];"
        );
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            table.Rows.Add(
                Enumerable.Range(0, reader.FieldCount)
                    .Select(
                        i => reader.IsDBNull(i) ? "" : reader.GetValue(i) is decimal number ? number.ToString("0.########", CultureInfo.InvariantCulture) : Convert.ToString(reader.GetValue(i), CultureInfo.InvariantCulture)
                    )
                    .ToArray()
            );
        }

        table.ExtendedProperties["SqlRevision"] = Convert.ToInt64(revision);
        table.AcceptChanges();
        return table;
    }

    public static DataTable LoadCatalog(string key)
    {
        using var c = Open();
        using var tx = c.BeginTransaction(IsolationLevel.Serializable);
        var result = ReadCatalog(c, tx, key);
        tx.Commit();
        return result;
    }

    private static void WriteCatalog(SqlConnection c, SqlTransaction tx, string key, DataTable table, long? expectedRevision)
    {
        var definition = Definition(key);
        if (!definition.Columns.Select(x => x.Header)
            .SequenceEqual(table.Columns.Cast<DataColumn>().Select(x => x.ColumnName)))
        {
            throw new InvalidDataException("Cấu trúc danh mục cũ không khớp: " + key);
        }

        using var version = Command(c, tx, "SELECT Revision FROM dbo.CatalogRevisions WITH(UPDLOCK,HOLDLOCK) WHERE ModuleKey=@key;");
        version.Parameters.Add("@key", SqlDbType.NVarChar, 64).Value = key;
        var current = version.ExecuteScalar();
        if (expectedRevision.HasValue && (current == null || Convert.ToInt64(current) != expectedRevision.Value))
        {
            throw new InvalidOperationException("Danh mục đã thay đổi ở cửa sổ khác. Hãy làm mới trước khi lưu.");
        }

        using (var clear = Command(c, tx, "DELETE FROM dbo.[" + definition.Table + "]"))
        {
            clear.ExecuteNonQuery();
        }

        using var insert = Command(
            c,
            tx,
            "INSERT dbo.[" + definition.Table + "] (" + string.Join(',', definition.Columns.Select(x => "[" + x.Name + "]")) + ") VALUES (" + string.Join(',', definition.Columns.Select((x, i) => "@p" + i)) + ");"
        );
        foreach (DataRow row in table.Rows)
        {
            if (row.RowState == DataRowState.Deleted)
            {
                continue;
            }

            insert.Parameters.Clear();
            for (int i = 0; i < definition.Columns.Length; i++)
            {
                var col = definition.Columns[i];
                var text = Convert.ToString(row[i]) ?? "";
                if (col.SqlType.StartsWith("decimal"))
                {
                    if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                    {
                        throw new InvalidOperationException("Giá trị không hợp lệ: " + col.Header);
                    }

                    var p = insert.Parameters.Add("@p" + i, SqlDbType.Decimal);
                    p.Precision = 28;
                    p.Scale = 8;
                    p.Value = value;
                }
                else if (col.SqlType == "int")
                {
                    if (!int.TryParse(text, out var value))
                    {
                        throw new InvalidOperationException("Giá trị không hợp lệ: " + col.Header);
                    }

                    insert.Parameters.Add("@p" + i, SqlDbType.Int).Value = value;
                }
                else
                {
                    insert.Parameters.Add("@p" + i, SqlDbType.NVarChar, -1).Value = text;
                }
            }

            insert.ExecuteNonQuery();
        }

        var next = current == null ? 0 : Convert.ToInt64(current) + 1;
        using var update = Command(
            c,
            tx,
            current == null ? "INSERT dbo.CatalogRevisions VALUES(@key,@revision)" : "UPDATE dbo.CatalogRevisions SET Revision=@revision WHERE ModuleKey=@key"
        );
        update.Parameters.Add("@key", SqlDbType.NVarChar, 64).Value = key;
        update.Parameters.Add("@revision", SqlDbType.BigInt).Value = next;
        update.ExecuteNonQuery();
    }

    public static void SaveCatalog(string key, DataTable table)
    {
        if (table.ExtendedProperties["SqlRevision"] is not long revision)
        {
            throw new InvalidOperationException("Hãy tải lại danh mục trước khi lưu.");
        }

        using var c = Open();
        using var tx = c.BeginTransaction(IsolationLevel.Serializable);
        WriteCatalog(
            c,
            tx,
            key,
            table,
            revision
        );
        tx.Commit();
        table.ExtendedProperties["SqlRevision"] = revision + 1;
        table.AcceptChanges();
    }

    private static void SeedUser(SqlConnection c, SqlTransaction tx, string username, string name, string role, string password)
    {
        const int iterations = 600000;
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32
        );
        using var cmd = Command(
            c,
            tx,
            "IF NOT EXISTS(SELECT 1 FROM dbo.UserAccounts WHERE Username=@username) INSERT dbo.UserAccounts VALUES(@username,@name,@role,@hash,@salt,@iterations,1)"
        );
        cmd.Parameters.Add("@username", SqlDbType.NVarChar, 128).Value = username;
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 256).Value = name;
        cmd.Parameters.Add("@role", SqlDbType.NVarChar, 16).Value = role;
        cmd.Parameters.Add("@hash", SqlDbType.VarBinary, 32).Value = hash;
        cmd.Parameters.Add("@salt", SqlDbType.VarBinary, 16).Value = salt;
        cmd.Parameters.Add("@iterations", SqlDbType.Int).Value = iterations;
        cmd.ExecuteNonQuery();
    }

    public static User? Authenticate(string username, string password)
    {
        using var c = Open();
        using var cmd = Command(
            c,
            null,
            "SELECT Username,DisplayName,Role,PasswordHash,Salt,Iterations FROM dbo.UserAccounts WHERE Username=@username AND Active=1"
        );
        cmd.Parameters.Add("@username", SqlDbType.NVarChar, 128).Value = username.Trim();
        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var role = reader.GetString(2);
        if (!Enum.TryParse<UserRole>(role, out var parsed))
        {
            return null;
        }

        var iterations = reader.GetInt32(5);
        if (iterations < 600000 || iterations > 10000000)
        {
            throw new InvalidDataException("Cấu hình mật khẩu trong DB không hợp lệ.");
        }

        var candidate = Rfc2898DeriveBytes.Pbkdf2(
            password,
            (byte[])reader[4],
            iterations,
            HashAlgorithmName.SHA256,
            32
        );
        return CryptographicOperations.FixedTimeEquals(candidate, (byte[])reader[3]) ? new User
        {
            Username = reader.GetString(0),
            DisplayName = reader.GetString(1),
            Role = parsed
        }

        : null;
    }

    private sealed class SavedCatalog
    {
        public List<string> Columns { get; set; } = [];
        public List<string[]> Rows { get; set; } = [];
    }

    public static void WritePortableBackup(Stream output, string assetsDirectory)
    {
        RestaurantState state;
        var catalogs = new Dictionary<string, SavedCatalog>();
        using (var c = Open())
        {
            using (var tx = c.BeginTransaction(IsolationLevel.Serializable))
            {
                state = ReadState(c, tx);
                foreach (var key in CatalogSchema.Modules.Keys)
                {
                    var t = ReadCatalog(c, tx, key);
                    catalogs[key] = new SavedCatalog
                    {
                        Columns = t.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList(),
                        Rows = t.Rows.Cast<DataRow>().Select(x => x.ItemArray.Select(v => v?.ToString() ?? "").ToArray()).ToList()
                    };
                }

                tx.Commit();
            }
        }

        using var zip = new ZipArchive(output, ZipArchiveMode.Create, true);
        void Write(string name, object value)
        {
            using var stream = zip.CreateEntry(name).Open();
            JsonSerializer.Serialize(stream, value, Json);
        }

        Write("restaurant.json", state);
        foreach (var catalog in catalogs)
        {
            Write("catalog-" + catalog.Key + ".json", catalog.Value);
        }

        Write(
            "database-info.json",
            new
            {
                Provider = "SQL Server",
                Database = DatabaseName,
                ExportedAt = DateTime.Now,
                Note = "Portable data export; use SQL Server Backup for a full database backup. Users and password hashes are excluded."
            }
        );
        var imageFolder = Path.Combine(assetsDirectory, "Images");
        if (Directory.Exists(imageFolder))
        {
            foreach (var file in Directory.GetFiles(imageFolder, "*", SearchOption.AllDirectories))
            {
                zip.CreateEntryFromFile(file, Path.GetRelativePath(assetsDirectory, file));
            }
        }
    }
}
