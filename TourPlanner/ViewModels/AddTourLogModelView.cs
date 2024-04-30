using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Bl;
using DAL;
using log4net;
using Models;
using TourPlanner.Views;
namespace TourPlanner.ViewModels
{
    public class AddTourLogModelView: ViewModelBase
    {

        public string _comment = "";
        public string _totalTime = "";
        public string _difficulty = "";
        public string _rating = "";
        public string _totalDistance = "";

        public Guid? logToModify;


        private string _tourerrorMessage = "";
        public string TourErrorMessage
        {
            get => _tourerrorMessage;
            set
            {
                if (_tourerrorMessage != value)
                {
                    _tourerrorMessage = value;
                    OnPropertyChanged(nameof(TourErrorMessage));
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

        public string Difficulty
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        public string Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public string TotalTime
        {
            get => _totalTime;
            set
            {
                if (_totalTime != value)
                {
                    _totalTime = value;
                    OnPropertyChanged(nameof(TotalTime));
                }
            }
        }

        public string TotalDistance
        {
            get => _totalDistance;
            set
            {
                if (_totalDistance != value)
                {
                    _totalDistance = value;
                    OnPropertyChanged(nameof(TotalDistance));
                }
            }
        }


        private readonly MainWindowViewModel _mainViewModel;

        public ICommand AddLogCommand { get; set; }
        public ICommand CancleLogCommand { get; set; }

        public AddTourLogModelView(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddLogCommand = new RelayCommand(o =>
            {
                CreateLogAction();
            });
            CancleLogCommand = new RelayCommand(o =>
            {
                _mainViewModel.LogVisibility = Visibility.Visible;
                _mainViewModel.AddLogVisibility = Visibility.Hidden;
            });
        }
        private void CreateLogAction()
        {
            if (logToModify != null)
            {
                _mainViewModel._tourLogService.DeleteTour((Guid)logToModify);
                logToModify = null;
            }

            if (TotalTime == "")
            {
                TourErrorMessage = "Duration not set";
            }
            else if (Comment == "")
            {
                TourErrorMessage = "Comment not set";
            }
            else if (TotalDistance == "")
            {
                TourErrorMessage = "Distance not set";
            }
            else if (Difficulty == "")
            {
                TourErrorMessage = "Distance not set";
            }
            else if (Rating == "")
            {
                TourErrorMessage = "Distance not set";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(TotalTime, "^[0-9]+$"))
            {
                TourErrorMessage = "Enter time in minutes";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(TotalDistance, @"^\d+(\,\d+)?$"))
            {
                TourErrorMessage = "Enter Distance in meters";
            }
            else
            {
                if (double.TryParse(TotalDistance, out double distance) && int.TryParse(TotalTime, out int time))
                {
                    _mainViewModel._tourLogService.AddTourLog(Comment, Difficulty, distance, time, Rating, _mainViewModel.TourVM.SelectedRoute.Id);
                }
                // refresh

                _mainViewModel.TourVM.SelectTour(_mainViewModel.TourVM.SelectedRoute.Id);

                _mainViewModel.LogVisibility = Visibility.Visible;
                _mainViewModel.AddLogVisibility = Visibility.Hidden;
                TotalTime = "";
                Comment = "";
                TotalDistance = "";
                Difficulty = "";
                Rating = "";
                TourErrorMessage = "";
            }

        }


    }
}

