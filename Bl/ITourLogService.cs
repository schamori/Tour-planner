﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public interface ITourLogService
    {
        List<TourLog> GetAllTourLogsForTour(Guid id);

        void AddTourLog(string comment, string difficulty, double totalDistance, int totalTime, string rating, Guid id);

        Route? GetTour(string tourName);
        void DeleteTour(string tourName);
        void ModifyTour();
    }
}
