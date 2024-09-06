using System.Data.Entity;
using System.Data.SQLite;
using SQLiteConnector.Migration;

namespace SQLiteConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();

            CreateTable createTable = new CreateTable();
            createTable.Connection = sqlite_conn;
            createTable.Execute();

            InjectData injectData = new InjectData();
            injectData.Connection = sqlite_conn;
            injectData.Execute();

            ReadData readData = new ReadData();
            readData.Connection = sqlite_conn;
            readData.Execute();
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source = database.db; Version = 3; New = True; Compress = True; ");
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
    }
}