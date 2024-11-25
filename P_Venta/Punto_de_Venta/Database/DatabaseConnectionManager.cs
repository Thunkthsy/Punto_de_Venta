using System; // Provides fundamental classes and base classes for .NET applications.
using MySql.Data.MySqlClient; // Allows working with MySQL database connections.

namespace Database
{
    // A static class that manages database connections for the application
    public static class DatabaseConnectionManager
    {
        // A read-only connection string to store database connection details
        private static readonly string connectionString;

        // Static constructor to initialize the connection string
        static DatabaseConnectionManager()
        {
            // Define database connection parameters
            string server = "localhost"; // The database server (local in this case)
            string user = "root"; // Username for accessing the database
            string pwd = "Ricardoydiego1."; // Password for the database user
            string database = "bd_punto_de_venta"; // Name of the database to connect to

            // Constructs the full connection string using the above parameters
            connectionString = $"Server={server};Database={database};User ID={user};Password={pwd};SslMode=none;";
        }

        // Property to access the connection string from other parts of the application
        public static string ConnectionString
        {
            get { return connectionString; }
        }

        // Method to create and return a new MySQL connection using the connection string
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString); // Returns a MySqlConnection object
        }
    }
}
