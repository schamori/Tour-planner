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

        private Visibility _logVisibility = Visibility.Visible;
        private Visibility _addLogVisibility = Visibility.Hidden;

        public ICommand AddLogCommand { get; set; }
        public ICommand CancleLogCommand { get; set; }
        public ICommand OpenAddLogCommand { get; set; }


        public string _comment;
        public string _duration;
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

        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public string Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }



        public TourLogsViewModel()
        {
            OpenAddLogCommand = new RelayCommand(OpenAddLog);
            TourLogs = new ObservableCollection<TourLog>();

            AddLogCommand = new RelayCommand(o =>
            {
                if (Comment == "")
                {
                    ErrorMessage = "Comment not set";
                }
                else if (Duration == "")
                {
                    ErrorMessage = "Duration not set";
                }
                else
                {
                    OnCreateLogButtonClick();

                }
                if (ErrorMessage == "")
                {
                    Comment = "";
                    Duration = "";
                    LogVisibility = Visibility.Hidden;
                    AddLogVisibility = Visibility.Visible;
                }
            });
        }

        public void OpenAddLog(object parameter)
        {
            var addLogWindow = new AddTourLog(); // Erstelle ein neues Fenster für das Hinzufügen von Logs
            addLogWindow.ShowDialog(); // Zeige das Fenster als Dialog
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

        private async void OnCreateLogButtonClick()
        {
            //_tourLogService.AddTourLog(route);
            LogVisibility = Visibility.Visible;
            AddLogVisibility = Visibility.Hidden;
        }

        private Visibility _addLogButtonVisibility = Visibility.Hidden;
        public Visibility AddLogButtonVisibility
        {
            get => _addLogButtonVisibility;
            set
            {
                if (_addLogButtonVisibility != value)
                {
                    _addLogButtonVisibility = value;
                    OnPropertyChanged(nameof(AddLogButtonVisibility));
                }
            }
        }

        public Visibility LogVisibility
        {
            get => _logVisibility;
            set
            {
                _logVisibility = value;
                OnPropertyChanged(nameof(LogVisibility));
            }
        }
        public Visibility AddLogVisibility
        {
            get => _addLogVisibility;
            set
            {
                _addLogVisibility = value;
                OnPropertyChanged(nameof(AddLogVisibility));
            }
        }
    }
}
