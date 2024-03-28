using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class TourRepo: ITourRepo
    {
        private const string CreateTourTableCommand = @"CREATE TABLE IF NOT EXISTS tours (t_id UUID PRIMARY KEY, t_name varchar UNIQUE, t_description varchar, t_creationTime timestamp, t_from varchar, t_to varchar, t_distance float, t_estimatedTime int, t_transport varchar);";
        private const string DeleteTourCommand = @"DELETE FROM Tours WHERE t_name = @t_name;";
        private const string AddCommand = @"INSERT INTO tours (t_id, t_name, t_description, t_distance, t_creationTime, t_estimatedTime, t_from, t_to, t_transport) VALUES ((@t_id), (@t_name), (@t_description), (@t_distance), (@t_creationTime) ,(@t_estimatedTime), (@t_from), (@t_to), (@t_transport));";
        private const string GetTourCommand = @"SELECT * FROM tours WHERE t_name = @t_name;";
        private const string UpdateCommand = @" UPDATE tours SET t_description = @t_description, t_distance = @t_distance, t_creationTime = @t_creationTime, t_estimatedTime = @t_estimatedTime, t_from = @t_from, t_to = @t_to, t_transport = @t_transport WHERE t_name = @t_name; ";

        private readonly string _connectionString;

        public TourRepo(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void Add(Route obj)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(AddCommand, connection);



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
        }

        public void DeleteTour(string tourName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteTourCommand, connection);

            cmd.Parameters.AddWithValue("t_name", tourName);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

        }

        public List<Route> GetAllTours()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM tours", connection);
            List<Route> tours = new();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tours.Add(new Route(
                        reader.GetGuid(reader.GetOrdinal("t_id")),
                        reader.GetString(reader.GetOrdinal("t_name")),
                        reader.GetString(reader.GetOrdinal("t_description")),
                        reader.GetString(reader.GetOrdinal("t_from")),
                        reader.GetString(reader.GetOrdinal("t_to")),
                        reader.GetString(reader.GetOrdinal("t_transport")),
                        reader.GetDouble(reader.GetOrdinal("t_distance")),
                        reader.GetInt32(reader.GetOrdinal("t_estimatedTime")),
                        reader.GetDateTime(reader.GetOrdinal("t_creationTime"))
                        ));
                }
                return tours;
            }
        }

        public Route? GetTour(string tourName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(GetTourCommand, connection);

            cmd.Parameters.AddWithValue("t_name", tourName);

            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new Route(
                        reader.GetGuid(reader.GetOrdinal("t_id")),
                        reader.GetString(reader.GetOrdinal("t_name")),
                        reader.GetString(reader.GetOrdinal("t_description")),
                        reader.GetString(reader.GetOrdinal("t_from")),
                        reader.GetString(reader.GetOrdinal("t_to")),
                        reader.GetString(reader.GetOrdinal("t_transport")),
                        reader.GetDouble(reader.GetOrdinal("t_distance")),
                        reader.GetInt32(reader.GetOrdinal("t_estimatedTime")),
                        reader.GetDateTime(reader.GetOrdinal("t_creationTime"))
                        );
        }

        public void UpdateTour(Route obj)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateCommand, connection);



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
