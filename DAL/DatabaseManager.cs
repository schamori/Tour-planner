using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DAL
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void OpenConnection(Action<NpgsqlConnection> actionWithConnection)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    actionWithConnection(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Handle exceptions (log them or show them to the user)
                }
            }
        }
    }
}
