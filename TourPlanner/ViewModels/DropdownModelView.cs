using Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using DAL;

namespace TourPlanner.ViewModels
{
    public class DropdownModelView
    {
        public readonly MainWindowViewModel _mainViewModel;

        public ICommand ImportCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand SummarizeReportCommand { get; set; }


        private void ExecuteImportCommand()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Filter = "DAT file (*.dat)|*.dat";
            if (openFileDialog.ShowDialog() == true)
            {
                List<Tour>? loadedTours = Deserialize<List<Tour>>(openFileDialog.FileName);

                string? responseMessage = null;

                if (loadedTours == null)
                    responseMessage = "Specified File is empty!";
                else
                {
                    foreach (Tour route in loadedTours)
                    {
                        try
                        {
                            _mainViewModel._tourService.AddTour(route, false);
                            _mainViewModel._routeService.CreateRouteAsync(route.Id, route.Name, route.Description, route.StartAddress, route.EndAddress, route.TransportType);
                        }
                        catch (RouteAlreadyExistsException)
                        {
                            responseMessage = "Import successfull but some or all Routes already existed";
                        }
                    }
                    // resfresh
                    _mainViewModel.TourVM.LoadAllTours();

                    MessageBox.Show(responseMessage ?? "Import successfull!");
                }
            }
        }
        
        private void ExecuteSaveCommand()
        {
            var Tours = _mainViewModel._tourService.GetAllTours();
            // Let user choose the location to save the PDF
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "DAT file (*.dat)|*.dat";
            if (saveFileDialog.ShowDialog() == true)
            {
                Serialize(Tours, saveFileDialog.FileName);
                MessageBox.Show("Tour statistics report has been saved successfully.");
            }
        }
        
        public DropdownModelView(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ImportCommand = new RelayCommand(o => { ExecuteImportCommand(); });
            SaveCommand = new RelayCommand(o => { ExecuteSaveCommand(); });
            SummarizeReportCommand = new RelayCommand(o => { ExecuteSummarizeReportCommand(); });
        }

        // Serialize an object to a file
        static void Serialize(object obj, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        // Deserialize an object from a file
        static T Deserialize<T>(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

       
        private void ExecuteSummarizeReportCommand()
        {
            Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)> tourStats = new Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)>();

            foreach (var tour in _mainViewModel.TourVM.Tours)
            {
                List<TourLog> tourLogs = _mainViewModel._tourLogService.GetAllTourLogsForTour(tour.Id);
                if (tourLogs.Count > 0)
                {
                    var averageDifficulty = tourLogs.Average(log => _mainViewModel.TourVM.DifficultyToDouble(log.Difficulty));
                    var averageTime = tourLogs.Average(log => log.TotalTime);
                    var averageDistance = tourLogs.Average(log => log.TotalDistance);
                    tourStats.Add(tour.Name, (averageDifficulty, averageTime, averageDistance));
                }
            }

            // Let user choose the location to save the PDF
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                _mainViewModel._exportService.CreateTourStatsPdf(tourStats, saveFileDialog.FileName);
                MessageBox.Show("Tour statistics report has been saved successfully.");

            }
        }


    }
}
