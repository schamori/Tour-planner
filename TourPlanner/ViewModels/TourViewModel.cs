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
    public class TourViewModel : ViewModelBase
    {

        private readonly MainWindowViewModel _mainViewModel;

        public ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> Tours
        {
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
        public ICommand DeleteCommand { get; private set; }

        public ICommand GotToAddCommand { get; set; }

        public ICommand ModifyCommand { get; set; }


        public TourViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            GotToAddCommand = new RelayCommand(o =>
            {
                _mainViewModel.ToursVisibility = Visibility.Hidden;
                _mainViewModel.AddTourVisibility = Visibility.Visible;
            });
            DeleteCommand = new RelayCommand(ExecuteDeleteTour);
            ModifyCommand = new RelayCommand(ModifyAction);
        }

        public void LoadAllTours()
        {
            List<Route> allTours = _mainViewModel._tourService.GetAllTours();
            Tours = new ObservableCollection<Tour>(allTours.Select(tour => new Tour { Name = tour.Name, Id = tour.Id }));
        }

        private void ExecuteDeleteTour(object obj)
        {
            Tour tour = obj as Tour;

            _mainViewModel._tourService.DeleteTour(tour!.Name);
            LoadAllTours();
            List<TourLog> tourLogs = _mainViewModel._tourLogService.GetAllTourLogsForTour(tour!.Id);
            tourLogs.ForEach(tourLog => _mainViewModel._tourLogService.DeleteTour(tourLog.Id));
        }
        private void ModifyAction(object parameter)
        {
            Tour tour = parameter as Tour;

            Route route = _mainViewModel._tourService.GetTour(tour!.Name)!;
            _mainViewModel.AddTourVM.nameToModify = route.Name;
            _mainViewModel.AddTourVM.Name = route.Name;
            _mainViewModel.AddTourVM.Description = route.Description;
            _mainViewModel.AddTourVM.From = route.StartAddress;
            _mainViewModel.AddTourVM.To = route.EndAddress;
            _mainViewModel.AddTourVM.TransportType = route.TransportType;
            _mainViewModel.ToursVisibility = Visibility.Hidden;
            _mainViewModel.AddTourVisibility = Visibility.Visible;
        }
        public void SelectTour(Guid tourId)
        {
            List<TourLog> allTourLogs = _mainViewModel._tourLogService.GetAllTourLogsForTour(tourId);
            _mainViewModel.TourLogsVM.SelectedTourLogs = new ObservableCollection<TourLog>(allTourLogs);
            _mainViewModel.AddLogButtonVisibility = Visibility.Visible;
            _mainViewModel.TourDetailsVisibility = Visibility.Visible;
            SelectedRoute = _mainViewModel._tourService.GetTourById(tourId)!;
        }

    }
    
}
