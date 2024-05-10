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
        private Visibility _mapVisibility = Visibility.Hidden;
        private Visibility _addTourVisibility = Visibility.Hidden;
        private Visibility _logVisibility = Visibility.Hidden;
        private Visibility _addLogVisibility = Visibility.Hidden;
        private Visibility _tourDetailsVisibility = Visibility.Hidden;
        private Visibility _addLogButtonVisibility = Visibility.Hidden;
        private Visibility _dropDownVisibility = Visibility.Hidden;



        public ITourService _tourService;
        public ITourLogService _tourLogService;
        public IRouteService _routeService;
        public IExportService _exportService;
        

        public AddTourViewModel AddTourVM { get; private set; }
        public TourLogsViewModel TourLogsVM { get; private set; }
        public TourViewModel TourVM { get; private set; }
        public AddTourLogModelView AddTourLogsVM  { get; private set; }
        public MapViewModel MapVM { get; private set; }
        public DropdownModelView DropDownVM { get; private set; }
        public MainWindowViewModel(ITourService tourService, ITourLogService tourLogService, IRouteService routeService, IExportService exportService)
        {
            _exportService = exportService;
            AddTourVM = new AddTourViewModel(this);
            TourLogsVM = new TourLogsViewModel(this);
            TourVM = new TourViewModel(this);
            AddTourLogsVM = new AddTourLogModelView(this);
            DropDownVM = new DropdownModelView(this);
            MapVM = new MapViewModel();
            _tourService = tourService;
            _tourLogService = tourLogService;
            _routeService = routeService;

            TourVM.TourSelected += MapVM.LoadMapImage;  // Subscribe to the TourSelected event

            TourVM.LoadAllTours();

        }
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

        public Visibility MapVisibility
        {
            get => _mapVisibility;
            set
            {
                _mapVisibility = value;
                OnPropertyChanged(nameof(MapVisibility));
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

        public Visibility DropDownVisibility
        {
            get => _dropDownVisibility;
            set
            {
                _dropDownVisibility = value;
                OnPropertyChanged(nameof(DropDownVisibility));
            }
        }
    }
}
