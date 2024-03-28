using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public class TourLogService : ITourLogService
    {
        private ITourLogRepo _tourLogRepo;
        public TourLogService(ITourLogRepo tourLogRepo)
        {
            _tourLogRepo = tourLogRepo;
        }
        public void AddTourLog(Guid id)
        {
            throw new NotImplementedException();
        }

        public void DeleteTour(string tourName)
        {
            throw new NotImplementedException();
        }

        public List<TourLog> GetAllTourLogsForTour(Guid id)
        {
            return _tourLogRepo.GetAllTourLogsForTour(id);
        }

        public Route? GetTour(string tourName)
        {
            throw new NotImplementedException();
        }

        public void ModifyTour()
        {
            throw new NotImplementedException();
        }
    }
}
