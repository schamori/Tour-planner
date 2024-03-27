using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
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

        // Beispiel für eine Methode, um alle Touren abzufragen
        public void GetAllTours()
        {
            OpenConnection(connection =>
            {
                using (var command = new NpgsqlCommand("SELECT * FROM tours", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Beispiel: Lese Daten und mache etwas damit
                            Console.WriteLine(reader["column_name"]); // Ersetze 'column_name' mit dem Namen der Spalte, die du auslesen möchtest
                        }
                    }
                }
            });
        }
    }
}
