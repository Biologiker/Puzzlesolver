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
        SQLiteConnection sqLiteConnection = new SQLiteConnection("Data Source = database.db; Version = 3; New = True; Compress = True; ");
        try
        {
            sqLiteConnection.Open();
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
        SQLiteCommand sqLiteCommand = _connection.CreateCommand();
        foreach (var value in data)
        {
            sqLiteCommand.CommandText = "INSERT INTO " + tableName + "(word) VALUES("+ value + ");";
            sqLiteCommand.ExecuteNonQuery();
        }
        
    }


    public static void RemoveData(string tableName)
    {
        SQLiteCommand sqLiteCommand = _connection.CreateCommand();
        sqLiteCommand.CommandText = "DELETE * FROM" + tableName + ";";
        sqLiteCommand.ExecuteNonQuery();

    }
}

