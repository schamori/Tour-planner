using Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TourLogsRepo : ITourLogRepo
    {
        private const string CreateTourTableCommand = @"CREATE TABLE IF NOT EXISTS tourlogs (tlog_id UUID PRIMARY KEY, tlog_comment varchar, tlog_creationTime timestamp, tlog_difficulty varchar, tlog_totaltime int, tlog_distance float, tlog_rating int, t_id UUID);";
        private const string DeleteTourCommand = @"DELETE FROM tourlogs WHERE tlog_id = @tlog_id;";
        private const string AddCommand = @"INSERT INTO tourlogs (t_id, t_name, t_description, t_distance, t_creationTime, t_estimatedTime, t_from, t_to, t_transport) VALUES ((@t_id), (@t_name), (@t_description), (@t_distance), (@t_creationTime) ,(@t_estimatedTime), (@t_from), (@t_to), (@t_transport));";
        private const string GetTourLogsCommand = @"SELECT * FROM tourlogs WHERE t_id = @t_id;";

        private readonly string _connectionString;

        public TourLogsRepo(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public void AddTourLog(TourLog obj)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(AddCommand, connection);

            cmd.Parameters.AddWithValue("tlog_id", obj.Id);
            cmd.Parameters.AddWithValue("tlog_comment", obj.Comment);
            cmd.Parameters.AddWithValue("tlog_creationTime", obj.Date);
            cmd.Parameters.AddWithValue("tlog_difficulty", obj.Difficulty);
            cmd.Parameters.AddWithValue("tlog_totaltime", obj.TotalTime);
            cmd.Parameters.AddWithValue("tlog_distance", obj.TotalDistance);
            cmd.Parameters.AddWithValue("tlog_rating", obj.Rating);
            cmd.Parameters.AddWithValue("t_id", obj.TourId);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
        }

        public void DeleteTourLog(Guid logId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(DeleteTourCommand, connection);

            cmd.Parameters.AddWithValue("tlog_id", logId);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

        }

        public List<TourLog> GetAllTourLogsForTour(Guid tourId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(GetTourLogsCommand, connection);
            List<TourLog> tours = new();
            cmd.Parameters.AddWithValue("t_id", tourId);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tours.Add(new TourLog(
                        reader.GetGuid(reader.GetOrdinal("tlog_id")),
                        reader.GetDateTime(reader.GetOrdinal("tlog_creationTime")),
                        reader.GetString(reader.GetOrdinal("tlog_comment")),
                        reader.GetString(reader.GetOrdinal("tlog_difficulty")),
                        reader.GetDouble(reader.GetOrdinal("tlog_distance")),
                        reader.GetInt32(reader.GetOrdinal("tlog_totaltime")),
                        reader.GetInt32(reader.GetOrdinal("tlog_rating")),
                        reader.GetGuid(reader.GetOrdinal("t_id"))
                        ));
                }
                return tours;
            }
        }

        public void UpdateTourLog()
        {
            throw new NotImplementedException();
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
