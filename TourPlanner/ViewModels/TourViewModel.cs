using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
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
using Microsoft.Win32;
using System.Windows;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace TourPlanner.ViewModels
{
    public class TourViewModel : ViewModelBase
    {

        public readonly MainWindowViewModel _mainViewModel;

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
        public event Action<Guid> TourSelected;
        private Tour _selectedroute;
        public Tour SelectedRoute
        {
            get => _selectedroute;
            set
            {
                if (_selectedroute != value)
                {
                    _selectedroute = value;
                    OnPropertyChanged(nameof(SelectedRoute));
                    TourSelected?.Invoke(_selectedroute.Id);
                }
            }

        }

        private string _tourSearchText = "";
        public string TourSearchText
        {
            get => _tourSearchText;
            set
            {
                if (_tourSearchText != value)
                {
                    _tourSearchText = value;
                    OnPropertyChanged(nameof(TourSearchText));
                }
            }
        }

        public void SearchTours()
        {
            LoadAllTours();
            var lowerCaseSearchText = TourSearchText.ToLower();
            Tours = new ObservableCollection<Tour>(
        Tours.Where(tour =>
            (tour.Name?.ToLower().Contains(lowerCaseSearchText) == true) ||
            (tour.StartAddress?.ToLower().Contains(lowerCaseSearchText) == true) ||
            (tour.EndAddress?.ToLower().Contains(lowerCaseSearchText) == true) ||
            (tour.TransportType?.ToLower().Contains(lowerCaseSearchText) == true) ||
            tour.Distance.ToString().ToLower().Contains(lowerCaseSearchText) ||
            tour.EstimatedTime.ToString().ToLower().Contains(lowerCaseSearchText) ||
            (tour.Description?.ToLower().Contains(lowerCaseSearchText) == true) ||
            (tour.TourLogs?.Any(log =>
                (log.Comment?.ToLower().Contains(lowerCaseSearchText) == true) ||
                (log.Difficulty?.ToLower().Contains(lowerCaseSearchText) == true) ||
                log.TotalDistance.ToString().ToLower().Contains(lowerCaseSearchText) ||
                log.TotalTime.ToString().ToLower().Contains(lowerCaseSearchText) ||
                log.Rating.ToString().ToLower().Contains(lowerCaseSearchText)) == true)
        )
    );
        }

        public ICommand DeleteCommand { get; private set; }

        public ICommand GotToAddCommand { get; set; }

        public ICommand ModifyCommand { get; set; }
        public ICommand SearchToursCommand { get; set; }

        public ICommand TourReportCommand { get; set; }


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
            SearchToursCommand = new RelayCommand(o =>
            {
                SearchTours();
            });

            TourReportCommand = new RelayCommand(o => { ExecuteTourReportCommand(); });


        }

  
        private void ExecuteTourReportCommand()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _mainViewModel._exportService.CreateTourReportPdf(SelectedRoute, saveFileDialog.FileName, Popularity, ChildFriendliness!);
                    MessageBox.Show("Tour report has been saved successfully.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save the document: " + ex.Message);
                }
            }
        }

        public void LoadAllTours()
        {
            List<Tour> allTours = _mainViewModel._tourService.GetAllTours();
            Tours = new ObservableCollection<Tour>(allTours.Select(tour => new Tour { Name = tour.Name, Id = tour.Id, Description = tour.Description,
                Distance = tour.Distance, EndAddress = tour.EndAddress, EstimatedTime = tour.EstimatedTime, StartAddress = tour.StartAddress, TransportType = tour.TransportType }));
        }

        private void ExecuteDeleteTour(object obj)
        {
            Tour tour = obj as Tour;

            _mainViewModel.TourDetailsVisibility = Visibility.Hidden;
            _mainViewModel.MapVisibility = Visibility.Hidden;
            _mainViewModel.LogVisibility = Visibility.Hidden;
            _mainViewModel.MapVM.DeleteMapImage(tour!.Id);
            _mainViewModel._tourService.DeleteTour(tour!.Name);
            LoadAllTours();
            List<TourLog> tourLogs = _mainViewModel._tourLogService.GetAllTourLogsForTour(tour!.Id);
            tourLogs.ForEach(tourLog => _mainViewModel._tourLogService.DeleteTour(tourLog.Id));
        }
        private void ModifyAction(object parameter)
        {
            Tour tour = parameter as Tour;

            Tour route = _mainViewModel._tourService.GetTour(tour!.Name)!;
            _mainViewModel.AddTourVM.nameToModify = route.Name;
            _mainViewModel.AddTourVM.Id = route.Id;
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

            var test = _mainViewModel._tourLogService.GetAllTourLogsForTour(tourId);
            SelectedRoute = _mainViewModel._tourService.GetTourById(tourId)!;
            _mainViewModel.AddLogButtonVisibility = Visibility.Visible;
            _mainViewModel.LogVisibility = Visibility.Visible;
            _mainViewModel.TourDetailsVisibility = Visibility.Visible;
            _mainViewModel.MapVisibility = Visibility.Visible;
            // Calculate Stats
            Popularity = SelectedRoute.TourLogs?.Count ?? 0;
            
            if (SelectedRoute.TourLogs?.Count > 0) {
                _mainViewModel.TourLogsVM.SelectedTourLogs = new ObservableCollection<TourLog>(SelectedRoute.TourLogs);
                var averageDifficulty = SelectedRoute.TourLogs.Average(log => DifficultyToDouble(log.Difficulty));
                var averageTime = SelectedRoute.TourLogs.Average(log => log.TotalTime);
                var averageDistance = SelectedRoute.TourLogs.Average(log => log.TotalDistance);

                ChildFriendliness = CalculateChildFriendliness(averageDifficulty, averageTime, averageDistance);
            } else
            {
                _mainViewModel.TourLogsVM.SelectedTourLogs = new ObservableCollection<TourLog>();
                ChildFriendliness = null;
            }

        }

        public double DifficultyToDouble(string difficulty)
        {
            switch (difficulty.ToUpper())
            {
                case "EASY":
                    return 1.0;
                case "MEDIUM":
                    return 2.0;
                case "HARD":
                    return 3.0;
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
        }

        private bool CalculateChildFriendliness(double difficulty, double totalTime, double distance)
        {
            return difficulty * 10 + totalTime / 30 + distance / 5 < 50;
        }


        // --- Stats ----

        private int _popularity;
        public int Popularity
        {
            get => _popularity;
            set
            {
                _popularity = value;
                OnPropertyChanged(nameof(Popularity));
            }
        }

        private bool? _childFriendliness;
        public bool? ChildFriendliness
        {
            get => _childFriendliness;
            set
            {
                _childFriendliness = value;
                OnPropertyChanged(nameof(ChildFriendliness));
            }
        }


    }

}
