using Bl;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Views;
using System.Xml.Linq;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        public ObservableCollection<TourLog> TourLogs { get; private set; }
        private ITourLogService _tourLogService;

        
        public string _errorMessage = "";

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }


        public TourLogsViewModel()
        {
            TourLogs = new ObservableCollection<TourLog>();

            
        }

        public void LoadLogsForTour(Guid tourId)
        {
            var logs = _tourLogService.GetAllTourLogsForTour(tourId);
            TourLogs.Clear();
            foreach (var log in logs)
            {
                TourLogs.Add(log);
            }
        }

        

        
    }
}
