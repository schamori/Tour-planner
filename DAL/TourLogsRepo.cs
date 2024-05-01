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
            var tour = _context.Routes.FirstOrDefault(t => t.Id == oldTourId);
            if (tour != null)
            {


                _context.SaveChanges();
            }
        }


    }

}
