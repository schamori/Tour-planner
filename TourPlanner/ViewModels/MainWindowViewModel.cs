﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bl;
using DAL;
using Models;

namespace TourPlanner.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private Visibility _toursVisibility = Visibility.Visible;
        private Visibility _addTourVisibility = Visibility.Hidden;

        public string _name;
        public string _description;
        public string _transportType;
        public string _from;
        public string _to;
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
        public ICommand ShowTourLogs { get; set; }

        public ICommand AddTourCommand { get; set; }

        public TourRepo _dbManager;


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
            Tours = new ObservableCollection<Tour>(allTours.Select(tour => new Tour { Name = tour.Name }));
        }

            public MainWindowViewModel(ITourService tourService, DatabaseManager _dbManager, ViewModelBase addTourViewModel)
        {
            _tourService = tourService;

            LoadAllTours();

            GotToAddCommand = new RelayCommand(o =>
            {
                ToursVisibility = Visibility.Hidden; 
                AddTourVisibility = Visibility.Visible;
            });

            DeleteCommand = new RelayCommand(DeleteAction);
            ModifyCommand = new RelayCommand(ModifyAction);
            
            AddTourCommand = new RelayCommand(o =>
            {
                if (Name == "")
                {
                    ErrorMessage = "Name not set";
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
                Name = "";
                Description = "";
                From = "";
                To = "";
                TransportType = "";
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
            Name = route.Name;
            Description = route.Description;
            From = route.StartAddress;
            To = route.EndAddress;
            TransportType = route.TransportType;
            ToursVisibility = Visibility.Hidden;
            AddTourVisibility = Visibility.Visible;
        }
        private async void OnCreateRouteButtonClick()
        {
            var routeService = new RouteService("5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0");
            Route route;
            if(TransportType == "Car") { TransportType = "driving-car"; }
            else if(TransportType == "Truck") { TransportType = "driving-truck"; }
            else if (TransportType == "Cycling") { TransportType = "cycling-regular"; }
            else if (TransportType == "Walking") { TransportType = "foot-walking"; }
            else { TransportType = "wheelchair"; }
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
            _tourService.AddTour(route);
            ToursVisibility = Visibility.Visible;
            AddTourVisibility = Visibility.Hidden;
            LoadAllTours();

        }

        public void SelectedTourLog(Guid tourId)
        {
            List<TourLog> allTourLogs = _tourLogService.GetAllTourLogsForTour(tourId);
            SelectedTourLogs = new ObservableCollection<TourLog>(allTourLogs);
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
    }
}
