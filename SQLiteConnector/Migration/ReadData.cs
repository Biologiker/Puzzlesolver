using System.Data.SQLite;

namespace SQLiteConnector.Migration
{
    public class ReadData
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
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = _connection.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                Console.WriteLine("Reading data from SampleTable:");
                while (sqlite_datareader.Read())
                {
                    string myreader = sqlite_datareader.GetString(0);
                    Console.WriteLine(myreader);
                }
                sqlite_datareader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
