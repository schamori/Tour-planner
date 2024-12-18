﻿using DAL;
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
        public void AddTourLog(string comment, string difficulty, double totalDistance, int totalTime, string rating, Guid id)
        {
            TourLog tourLog = new TourLog (Guid.NewGuid(), DateTime.Now, comment, difficulty, totalDistance, totalTime, rating, id);
            _tourLogRepo.AddTourLog(tourLog);
        }

        public void DeleteTour(Guid tourId)
        {
            _tourLogRepo.DeleteTourLog(tourId);
        }

        public List<TourLog> GetAllTourLogsForTour(Guid id)
        {
            return _tourLogRepo.GetAllTourLogsForTour(id);
        }

        public TourLog? GetLog(Guid id)
        {
            return _tourLogRepo.GetSingleLog(id);
        }

        public void ModifyTour()
        {
            throw new NotImplementedException();
        }

        public void UpdateLogId(Guid oldTourId, Guid tourId)
        {
            _tourLogRepo.UpdateLogId(oldTourId, tourId);
        }
    }
}
