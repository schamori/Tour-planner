using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class TourRepo
    {
        private const string CreateTourTableCommand = @"CREATE TABLE IF NOT EXISTS tours (t_id UUID PRIMARY KEY, t_name varchar, t_description varchar, t_creationTime timestamp, t_from varchar, t_to varchar, t_distance float, t_estimatedTime int, t_transport varchar);";
        private readonly string _connectionString;

        public TourRepo(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public Route? Add(Route obj)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO tours (t_id, t_name, t_description, t_distance, t_creationTime, t_estimatedTime, t_from, t_to, t_transport) VALUES ((@t_id), (@t_name), (@t_description), (@t_distance), (@t_creationTime) ,(@t_estimatedTime), (@t_from), (@t_to), (@t_transport))", connection);

            cmd.Parameters.AddWithValue("t_id", obj.Id);
            cmd.Parameters.AddWithValue("t_name", obj.Name);
            cmd.Parameters.AddWithValue("t_description", obj.Description);
            cmd.Parameters.AddWithValue("t_distance", obj.Distance);
            cmd.Parameters.AddWithValue("t_creationTime", obj.CreationDate);
            cmd.Parameters.AddWithValue("t_estimatedTime", obj.EstimatedTime);
            cmd.Parameters.AddWithValue("t_from", obj.StartAddress);
            cmd.Parameters.AddWithValue("t_to", obj.EndAddress);
            cmd.Parameters.AddWithValue("t_transport", obj.TransportType);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        private void EnsureTables()
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateTourTableCommand, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
