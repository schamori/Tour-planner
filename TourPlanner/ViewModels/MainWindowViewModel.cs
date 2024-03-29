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
using static System.Net.Mime.MediaTypeNames;

namespace TourPlanner.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private Visibility _toursVisibility = Visibility.Visible;
        private Visibility _addTourVisibility = Visibility.Hidden;
        private Visibility _logVisibility = Visibility.Visible;
        private Visibility _addLogVisibility = Visibility.Hidden;

        public string _name = "";
        public string _description = "";
        public string _transportType = "";
        public string _from = "";
        public string _to = "";
        public string _errorMessage = "";
        public string _comment = "";
        public string _totalTime = "";
        public string _difficulty = "";
        public string _rating = "";
        public string _totalDistance = "";
        public Guid _id;

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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public string TransportType
        {
            get => _transportType;
            set
            {
                if (_transportType != value)
                {
                    _transportType = value;
                    OnPropertyChanged(nameof(TransportType));
                }
            }
        }
        public string From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged(nameof(From));
                }
            }
        }
        public string To
        {
            get => _to;
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged(nameof(To));
                }
            }
        }

        public ICommand GotToAddCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ModifyCommand { get; set; }

        public ICommand AddTourCommand { get; set; }
        public ICommand AddLogCommand { get; set; }
        public ICommand CancleLogCommand { get; set; }
        public ICommand OpenAddLogCommand { get; set; }

        public ICommand GoToAddLogCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ICommand OnTourClick { get; set; }

        public TourRepo _dbManager;

        private string nameToModify = "";
        public class Tour
        {
            public string Name { get; set; }
            public Guid Id { get; set; }
        }

        public class ToursLog
        {
            public Guid Id { get; set; }
        }


        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours {
            get => _tours;
            set
            {
                if (_tours != value)
                {
                    _tours = value;
                    OnPropertyChanged(nameof(Tours));
                }
            }
        }

        private Route _selectedroute;
        public Route SelectedRoute
        {
            get => _selectedroute;
            set
            {
                if (_selectedroute != value)
                {
                    _selectedroute = value;
                    OnPropertyChanged(nameof(SelectedRoute));
                }
            }

        }

        private ObservableCollection<TourLog> _selectedTourLogs;
        public ObservableCollection<TourLog> SelectedTourLogs
        {
            get => _selectedTourLogs;
            set
            {
                if (_selectedTourLogs != value)
                {
                    _selectedTourLogs = value;
                    OnPropertyChanged(nameof(SelectedTourLogs));
           
                }
            }
        }

        private ITourService _tourService;
        private ITourLogService _tourLogService;
        public void LoadAllTours()
        {
            List<Route> allTours = _tourService.GetAllTours();
            Tours = new ObservableCollection<Tour>(allTours.Select(tour => new Tour { Name = tour.Name, Id = tour.Id }));
        }

        public MainWindowViewModel(ITourService tourService, ITourLogService tourLogService, DatabaseManager _dbManager, ViewModelBase addTourViewModel)
        {
            _tourService = tourService;
            _tourLogService = tourLogService;

            LoadAllTours();

            OnTourClick = new RelayCommand(SelectTour);
            GotToAddCommand = new RelayCommand(o =>
            {
                ToursVisibility = Visibility.Hidden; 
                AddTourVisibility = Visibility.Visible;
            });
            GoBackCommand = new RelayCommand(o =>
            {
                ToursVisibility = Visibility.Visible;
                AddTourVisibility = Visibility.Hidden;
            });
            DeleteCommand = new RelayCommand(DeleteAction);
            ModifyCommand = new RelayCommand(ModifyAction);
            
            AddTourCommand = new RelayCommand(o =>
            {
                if (nameToModify != "")
                    _tourService.DeleteTour(nameToModify);
                if (Name == "")
                {
                    ErrorMessage = "Name not set";
                } else if (_tourService.GetTour(Name) != null )
                {
                    ErrorMessage = "Name already taken";
                }
                else if (Description == "")
                {
                    ErrorMessage = "Description not set";
                }
                else if (From == "")
                {
                    ErrorMessage = "From location not set";
                }
                else if (To == "")
                {
                    ErrorMessage = "To location not set";
                }
                else if (TransportType == "")
                {
                    ErrorMessage = "TransportType not set";
                } else 
                {
                     OnCreateRouteButtonClick();
                }
                
            });

            GoToAddLogCommand = new RelayCommand(o =>
            {
                LogVisibility = Visibility.Hidden;
                AddLogVisibility = Visibility.Visible;
            });

            CancleLogCommand = new RelayCommand(o =>
            {
                LogVisibility = Visibility.Visible;
                AddLogVisibility = Visibility.Hidden;
            });

            AddLogCommand = new RelayCommand(o =>
            {
                if (TotalTime == "")
                {
                    ErrorMessage = "Duration not set";
                }
                else if (Comment == "")
                {
                    ErrorMessage = "Comment not set";
                }
                else if (TotalDistance == "")
                {
                    ErrorMessage = "Distance not set";
                }
                else if (Difficulty == "")
                {
                    ErrorMessage = "Distance not set";
                }
                else if (Rating == "")
                {
                    ErrorMessage = "Distance not set";
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(TotalTime, "^[0-9]+$"))
                {
                    ErrorMessage = "Enter time in minutes";
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(TotalDistance, @"^\d+(\,\d+)?$"))
                {
                    ErrorMessage = "Enter Distance in meters";
                }
                else
                {
                    OnCreateLogButtonClick();

                }
                if (ErrorMessage == "")
                {
                    TotalTime = "";
                    TotalDistance = "";
                    Comment = "";
                    Difficulty = "";
                    Rating = "";
                }

            });
        }

        private void DeleteAction(object parameter)
        {
            Tour tour = parameter as Tour;

            _tourService.DeleteTour(tour!.Name);
            LoadAllTours();
        }

        private void ModifyAction(object parameter)
        {
            Tour tour = parameter as Tour;

            Route route = _tourService.GetTour(tour!.Name)!;
            nameToModify = route.Name;
            Name = route.Name;
            Description = route.Description;
            From = route.StartAddress;
            To = route.EndAddress;
            TransportType = route.TransportType;
            ErrorMessage = "";
            ToursVisibility = Visibility.Hidden;
            AddTourVisibility = Visibility.Visible;
        }

        private void SelectTour(object sender)
        {
            var listBox = sender as ListBox;
            var selectedTour = listBox.SelectedItem as TourPlanner.ViewModels.MainWindowViewModel.Tour;

            if (selectedTour != null)
            {
                ;

            }
        }
        private async void OnCreateRouteButtonClick()
        {
            var routeService = new RouteService("5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0");
            Route route;
           
            try
            {
                route = await routeService.CreateRouteAsync(Name, Description, From, To, TransportType);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            catch (System.ArgumentNullException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            _tourService.AddTour(route);
            ToursVisibility = Visibility.Visible;
            AddTourVisibility = Visibility.Hidden;
            Name = "";
            Description = "";
            From = "";
            To = "";
            TransportType = "";
            LoadAllTours();

        }

        public void SelectedTourLog(Guid tourId)
        {
            List<TourLog> allTourLogs = _tourLogService.GetAllTourLogsForTour(tourId);
            SelectedTourLogs = new ObservableCollection<TourLog>(allTourLogs);
            Id = tourId;
            AddLogButtonVisibility = Visibility.Visible;
            TourDetailsVisibility = Visibility.Visible;
            SelectedRoute = _tourService.GetTourById(tourId)!;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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

        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private Visibility _tourDetailsVisibility = Visibility.Hidden;

        public Visibility TourDetailsVisibility
        {
            get => _tourDetailsVisibility;
            set
            {
                _tourDetailsVisibility = value;
                OnPropertyChanged(nameof(TourDetailsVisibility));
            }
        }
        public Visibility ToursVisibility
        {
            get => _toursVisibility;
            set
            {
                _toursVisibility = value;
                OnPropertyChanged(nameof(ToursVisibility));
            }
        }
        public Visibility AddTourVisibility
        {
            get => _addTourVisibility;
            set
            {
                _addTourVisibility = value;
                OnPropertyChanged(nameof(AddTourVisibility));
            }
        }

        private Visibility _addLogButtonVisibility = Visibility.Collapsed;
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

        private void OnCreateLogButtonClick()
        {

            if (double.TryParse(TotalDistance, out double distance) && int.TryParse(TotalTime, out int time))
            {
                _tourLogService.AddTourLog(Comment, Difficulty, distance, time, Rating, Id);
            }
            SelectedTourLogs.Clear();
            LogVisibility = Visibility.Visible;
            AddLogVisibility = Visibility.Hidden;
            
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
