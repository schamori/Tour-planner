using System;
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

        public TourRepo _dbManager;


        public class Tour
        {
            public string Name { get; set; }
        }

        public ObservableCollection<Tour> Tours { get; set; }

        public void LoadAllTours()
        {
            var _tourManager = new TourRepo("Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin");
            List<Route> allTours = _tourManager.GetAllTours();
            Tours = new ObservableCollection<Tour>(allTours.Select(tour => new Tour { Name = tour.Name }));
        }

            public MainWindowViewModel(DatabaseManager _dbManager, ViewModelBase addTourViewModel)
        {
            LoadAllTours();


            GotToAddCommand = new RelayCommand(o =>
            {
                ToursVisibility = Visibility.Hidden; 
                AddTourVisibility = Visibility.Visible;
            });
            AddCommand = new RelayCommand(o =>
            {
            });
            DeleteCommand = new RelayCommand(o =>
            {
            });
            ModifyCommand = new RelayCommand(o =>
            {
            });
            AddTourCommand = new RelayCommand(o =>
            {
                ToursVisibility = Visibility.Visible;
                AddTourVisibility = Visibility.Hidden;

                OnCreateRouteButtonClick();
            });
        }

        private async void OnCreateRouteButtonClick()
        {
            var _tourManager = new TourRepo("Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin");
            var routeService = new RouteService("5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0");
            Route route = await routeService.CreateRouteAsync(Name, Description, From, To, TransportType);
            // Aktualisiere die GUI mit den erhaltenen Routendaten und dem Bild
            _tourManager.Add(route);
            LoadAllTours();
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
