﻿using Bl;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        public ObservableCollection<TourLog> TourLogs { get; private set; }
        private ITourLogService _tourLogService;

        public TourLogsViewModel()
        {
            TourLogs = new ObservableCollection<TourLog>();
        }

        public void LoadLogsForTour(Guid tourId)
        {
            _tourLogService.GetAllTourLogsForTour(tourId);
        }

        public void AddLog(TourLog log)
        {
            // ToDo
            TourLogs.Add(log);
        }
    }
}
