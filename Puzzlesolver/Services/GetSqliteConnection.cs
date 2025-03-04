using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace Puzzlesolver.Services;

public abstract class GetSqliteConnection
{
    
    private static SQLiteConnection _connection;

    public GetSqliteConnection()
    {
        _connection = GetConnection();
    }
    public static SQLiteConnection GetConnection()
    {
        SQLiteConnection sqLiteConnection = new SQLiteConnection(@"Data Source=../SQLiteConnector/database.db;Version=3;Compress=True;");
        try
        {
            sqLiteConnection.Open();
            var cmd = sqLiteConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM SampleTable;";
            Console.WriteLine(cmd.ExecuteNonQuery());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return sqLiteConnection;
    }
    
    public static void InsertWords(string tableName, List<string> data)
    {
        using (var command = _connection.CreateCommand())
        {
            var sql = command.CommandText = $"INSERT INTO {tableName} (word) VALUES (@word);";
            Console.WriteLine(sql);

            foreach (var value in data)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@word", value);
                command.ExecuteNonQuery();
            }
        }
    }



    public static void RemoveData(string tableName)
    {
        SQLiteCommand sqLiteCommand = _connection.CreateCommand();
        sqLiteCommand.CommandText = "DELETE * FROM" + tableName + ";";
        sqLiteCommand.ExecuteNonQuery();

    }
}

