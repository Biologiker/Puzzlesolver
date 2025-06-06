using System.Data.SQLite;

namespace SQLiteConnector;

internal class Program
{
    private static void Main(string[] args)
    {
        SQLiteConnection sqlite_conn;
        sqlite_conn = CreateConnection();
        CreateTable(sqlite_conn);
        InsertData(sqlite_conn);
        ReadData(sqlite_conn);
    }

    private static SQLiteConnection CreateConnection()
    {
        SQLiteConnection sqlite_conn;
        // Create a new database connection:
        sqlite_conn = new SQLiteConnection(@"Data Source=C:\Users\mo_ha\lf12\Puzzlesolver\SQLiteConnector\database.db;Version=3;Compress=True;");
        // Open the connection:
        try
        {
            sqlite_conn.Open();
        }
        catch (Exception ex)
        {
        }

        return sqlite_conn;
    }

    private static void CreateTable(SQLiteConnection conn)
    {
        SQLiteCommand sqlite_cmd;
        var Createsql = "CREATE TABLE SampleTable (Col1 VARCHAR(20), Col2 INT)";
        var Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
        sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = Createsql;
        sqlite_cmd.ExecuteNonQuery();
        sqlite_cmd.CommandText = Createsql1;
        sqlite_cmd.ExecuteNonQuery();
    }

    private static void InsertData(SQLiteConnection conn)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
        sqlite_cmd.ExecuteNonQuery();
        sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
        sqlite_cmd.ExecuteNonQuery();
        sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
        sqlite_cmd.ExecuteNonQuery();


        sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
        sqlite_cmd.ExecuteNonQuery();
    }

    private static void ReadData(SQLiteConnection conn)
    {
        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        while (sqlite_datareader.Read())
        {
            var myreader = sqlite_datareader.GetString(0);
            Console.WriteLine(myreader);
        }

        conn.Close();
    }
}