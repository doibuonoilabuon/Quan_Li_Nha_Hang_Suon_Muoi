using System.Data;
using Microsoft.Data.SqlClient;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Database;


public class DBProcess : IDisposable
{
    public static string ChuoiKetNoi { get; set; } =
        @"Data Source=WHISPERSTONE\SQLEXPRESS01;Initial Catalog=LuaMocDB;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=10";

    private SqlConnection? sqlConnect;
    private SqlTransaction? giaoDich;

    private void OpenConnect()
    {
        if (sqlConnect?.State == ConnectionState.Open)
            return;
        sqlConnect = new SqlConnection(ChuoiKetNoi);
        try { sqlConnect.Open(); }
        catch { CloseConnect(); throw; }
    }

    private void CloseConnect()
    {
        sqlConnect?.Dispose();
        sqlConnect = null;
    }

    
    public DataTable DataReader(string sqlQuery, params SqlParameter[] parameters)
    {
        OpenConnect();
        try
        {
            using var command = new SqlCommand(sqlQuery, sqlConnect, giaoDich);
            command.Parameters.AddRange(parameters);
            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        finally
        {
            if (giaoDich == null) CloseConnect();
        }
    }

    
    public int DataChange(string sqlQuery, params SqlParameter[] parameters)
    {
        OpenConnect();
        try
        {
            using var command = new SqlCommand(sqlQuery, sqlConnect, giaoDich);
            command.Parameters.AddRange(parameters);
            return command.ExecuteNonQuery();
        }
        finally
        {
            if (giaoDich == null) CloseConnect();
        }
    }

    
    public void BatDauGiaoDich(IsolationLevel mucKhoa = IsolationLevel.Serializable)
    {
        if (giaoDich != null) throw new InvalidOperationException("Đang có giao dịch chưa kết thúc.");
        OpenConnect();
        try { giaoDich = sqlConnect!.BeginTransaction(mucKhoa); }
        catch { CloseConnect(); throw; }
    }

    public void ChapNhanGiaoDich()
    {
        if (giaoDich == null) throw new InvalidOperationException("Chưa bắt đầu giao dịch.");
        try { giaoDich.Commit(); }
        finally { giaoDich.Dispose(); giaoDich = null; CloseConnect(); }
    }

    public void HuyGiaoDich()
    {
        if (giaoDich == null) return;
        try { giaoDich.Rollback(); }
        finally { giaoDich.Dispose(); giaoDich = null; CloseConnect(); }
    }

    public void Dispose()
    {
        try { HuyGiaoDich(); }
        finally { CloseConnect(); }
    }
}
