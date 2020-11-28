using System;
using MySql.Data.MySqlClient;
using System.IO;
using System.Diagnostics;

namespace Maple2.Data.Utils
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Call for connection in UserStorage
        public string connectionString;

        // Constructor
        public DBConnect()
        {
            Initialize();
        }

        // Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "ms2_db";
            uid = "root";
            password = "";
            
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        // Open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                // 0: Cannot connect to server.
                // 1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        // Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // Insert statement
        public void Insert()
        {
            // Here will manage the Insert values. Not tested yet
            string query = "INSERT INTO map (MapID) VALUES(2000062)";

            // Open connection
            if (this.OpenConnection() == true)
            {
                // Create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Execute command
                cmd.ExecuteNonQuery();

                // Close connection
                this.CloseConnection();
            }
        }

        // Update statement
        public void Update()
        {
            // Here will manage the Update values. Not tested yet
            string query = "UPDATE map SET MapID WHERE idmap=1";

            // Open connection
            if (this.OpenConnection() == true)
            {
                // Create mysql command
                MySqlCommand cmd = new MySqlCommand();
                // Assign the query using CommandText
                cmd.CommandText = query;
                // Assign the connection using Connection
                cmd.Connection = connection;

                // Execute query
                cmd.ExecuteNonQuery();

                // Close connection
                this.CloseConnection();
            }
        }

        // Delete statement
        public void Delete()
        {
            // Here will manage the Deletes values. Not tested yet
            string query = "DELETE FROM map WHERE MapID=2000062";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        // Select statement. Here is where the magic is.
        // Making this many methods accepting parameters
        // OR
        // Making this as a Request Context to adapt as it needs.

        public int Select()
        {
            string query = "SELECT MapID FROM map";

            // Create a int to store the result
            int mapid=0;

            // Open connection
            if (this.OpenConnection() == true)
            {
                // Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    mapid = dataReader.GetInt32(0);
                }

                // Close Data Reader
                dataReader.Close();

                // Close Connection
                this.CloseConnection();

                // Return id of the map
                return mapid;
            }
            else
            {
                return 0;
            }
        }

        public int Select(int character)
        {
            int sum = character + 10;
            return sum;
        }

        // Count statement
        public int Count()
        {
            // This retrieves how many MapID are. Not need for now.
            string query = "SELECT Count(MapID) FROM map";
            int Count = -1;

            // Open Connection
            if (this.OpenConnection() == true)
            {
                // Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                // Close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        // Backup
        // This is not the final version. but works for now.
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                // Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);

                // Using Process as part of the System.Diagnostics for basic usage.
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                // Here execute the process for Backup MYSQL's Database.
                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error , unable to backup! "+ex);
            }
        }

        // Restore
        // Not an expert in here neither. This is setup as from backup but in reverse.
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error , unable to Restore! "+ex);
            }
        }

    }
}
