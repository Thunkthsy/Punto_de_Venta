using System;
using MySql.Data.MySqlClient;

namespace Database
{
    public static class DatabaseConnectionManager
    {
        private static readonly string connectionString;

        static DatabaseConnectionManager()
        {
            string server = "localhost";
            string user = "root";
            string pwd = "Ricardoydiego1.";
            string database = "bd_punto_de_venta";

            connectionString = $"Server={server};Database={database};User ID={user};Password={pwd};SslMode=none;";
        }

        public static string ConnectionString
        {
            get { return connectionString; }
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
