using Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class TourLogsRepo : ITourLogRepo
    {
        private const string CreateTourTableCommand = @"CREATE TABLE IF NOT EXISTS tourlogs (tlog_id UUID PRIMARY KEY, tlog_comment varchar, tlog_creationTime timestamp, tlog_difficulty varchar, tlog_totaltime int, tlog_distance float, tlog_rating varchar, t_id UUID);";
        private const string DeleteTourCommand = @"DELETE FROM tourlogs WHERE tlog_id = @tlog_id;";
        private const string AddCommand = @"INSERT INTO tourlogs (tlog_id, tlog_comment, tlog_creationTime, tlog_difficulty, tlog_totaltime, tlog_distance, tlog_rating, t_id) VALUES ((@tlog_id), (@tlog_comment), (@tlog_creationTime), (@tlog_difficulty), (@tlog_totaltime) ,(@tlog_distance), (@tlog_rating), (@t_id));";
        private const string GetTourLogsCommand = @"SELECT * FROM tourlogs WHERE t_id = @t_id;";
        private const string GetSingleLogCommand = @"SELECT * FROM tourlogs WHERE tlog_id = @tlog_id;";
        private const string UpdateCommand = @"UPDATE tourlogs SET t_id = @t_idnew WHERE t_id = @t_idold; ";

        private readonly AppDbContext _context;

        public TourLogsRepo(AppDbContext context)
        {
            _context = context;
        }
        public void AddTourLog(TourLog obj)
        {
            _context.TourLogs.Add(obj);
            _context.SaveChanges();
        }

        public void DeleteTour()
        {
            throw new NotImplementedException();
        }

        public void DeleteTourLog(Guid logId)
        {
            var tourLog = _context.TourLogs.Find(logId);
            if (tourLog != null)
            {
                _context.TourLogs.Remove(tourLog);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Tour log not found");
            }

        }

        public List<TourLog> GetAllTourLogsForTour(Guid tourId)
        {
            return _context.TourLogs
                       .Where(t => t.TourId == tourId)
                       .ToList();
        }


        public TourLog? GetSingleLog(Guid id)
        {
            return _context.TourLogs.Find(id);
        }

        public void UpdateLogId(Guid oldTourId, Guid tourId)
        {
             /* using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateCommand, connection);

            cmd.Parameters.AddWithValue("t_idold", oldTourId);
            cmd.Parameters.AddWithValue("t_idnew", tourId);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery(); */
        }

        public void UpdateTourLog()
        {
            throw new NotImplementedException();
        }
    }

}
