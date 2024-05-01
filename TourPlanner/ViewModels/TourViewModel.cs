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
                    TourSelected?.Invoke(_selectedroute.Id);
                }
            }

        }

        private ObservableCollection<Tour> _filteredTours;
        public ObservableCollection<Tour> FilteredTours
        {
            get => _filteredTours;
            set
            {
                _filteredTours = value;
                OnPropertyChanged(nameof(FilteredTours));
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
            Tours = new ObservableCollection<Tour>(Tours.Where(tour =>
                tour.Name.ToLower().Contains(lowerCaseSearchText)));
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

        public void CreateTourReportPdf(Route tour, string filePath)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Tour Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Arial", 12, XFontStyle.BoldItalic);
            XFont font = new XFont("Arial", 10, XFontStyle.Regular);

            gfx.DrawString("Tour Report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Tours", titleFont, XBrushes.Black, new XRect(20, 40, page.Width, page.Height), XStringFormats.TopLeft);

            int yPoint = 60;

            // Print Tour Details
            gfx.DrawString($"Tour Name: {tour.Name}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Description: {tour.Description}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Start Address: {tour.StartAddress}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"End Address: {tour.EndAddress}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Transport Type: {tour.TransportType}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Distance: {tour.Distance} km", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Estimated Time: {tour.EstimatedTime} minutes", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Creation Date: {tour.CreationDate.ToShortDateString()}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;

            
            gfx.DrawString( $"Popularity: {Popularity}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;

            gfx.DrawString($"ChildFriendliness: {ChildFriendliness}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            yPoint += 40;

            gfx.DrawString("Tour Logs", titleFont, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            yPoint += 20;

            // Print Tour Logs
            if (tour.TourLogs != null && tour.TourLogs.Count > 0)
            {
                foreach (var log in tour.TourLogs)
                {
                    yPoint += 20;
                    gfx.DrawString($"Date: {log.Date.ToShortDateString()} - Comment: {log.Comment} - Difficulty: {log.Difficulty} - Distance: {log.TotalDistance} km - Time: {log.TotalTime} minutes - Rating: {log.Rating}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
                }
            }
            else
            {
                gfx.DrawString("No tour logs available.", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            }

            // Save the document
            document.Save(filePath);
            MessageBox.Show("Tour report has been saved successfully.");
        }
        private void ExecuteTourReportCommand()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    CreateTourReportPdf(SelectedRoute, saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save the document: " + ex.Message);
                }
            }
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
            SelectedRoute = _mainViewModel._tourService.GetTourById(tourId)!;
            _mainViewModel.AddLogButtonVisibility = Visibility.Visible;
            _mainViewModel.TourDetailsVisibility = Visibility.Visible;
            _mainViewModel.MapVisibility = Visibility.Visible;
            // Calculate Stats
            Popularity = SelectedRoute.TourLogs?.Count ?? 0;
            
            if (SelectedRoute.TourLogs != null) {
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
