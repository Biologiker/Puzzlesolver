using System.Data.SQLite;

namespace SQLiteConnector.Migration
{
    public class CreateTable
    {
        private SQLiteConnection _connection;

        // Getter and Setter for the connection
        public SQLiteConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public void Execute()
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                string Createsql = "CREATE TABLE IF NOT EXISTS SampleTable (Col1 VARCHAR(20), Col2 INT)";
                string Createsql1 = "CREATE TABLE IF NOT EXISTS SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
                sqlite_cmd = _connection.CreateCommand();
                sqlite_cmd.CommandText = Createsql;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = Createsql1;
                sqlite_cmd.ExecuteNonQuery();
                Console.WriteLine("Tables created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }
        }
    }
}
