using System.Data.SQLite;

namespace SQLiteConnector.Migration
{
    public class InjectData
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
                sqlite_cmd = _connection.CreateCommand();

                sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text', 1);";
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1', 2);";
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2', 3);";
                sqlite_cmd.ExecuteNonQuery();

                sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3', 3);";
                sqlite_cmd.ExecuteNonQuery();

                Console.WriteLine("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting data: " + ex.Message);
            }
        }
    }
}
