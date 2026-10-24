using System.Data;
using Microsoft.Data.SqlClient;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;


public class ProcessDatabase
{
    public static string ChuoiKetNoi
    {
        get;
        set;
    } =
        @"Data Source=WHISPERSTONE\SQLEXPRESS01;Initial Catalog=LuaMocDB;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=10";

    public SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(ChuoiKetNoi);
        try
        {
            connection.Open();
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }

    public DataTable DataReader(string sqlQuery, params SqlParameter[] parameters)
    {
        using var connection = OpenConnection();
        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddRange(parameters);
        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public int DataChange(string sqlQuery, params SqlParameter[] parameters)
    {
        using var connection = OpenConnection();
        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddRange(parameters);
        return command.ExecuteNonQuery();
    }
}
