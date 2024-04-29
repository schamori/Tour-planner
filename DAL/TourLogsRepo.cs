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
