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
                List<Route>? loadedTours = Deserialize<List<Route>>(openFileDialog.FileName);

                string? responseMessage = null;

                if (loadedTours == null)
                    responseMessage = "Specified File is empty!";
                else
                {
                    foreach (Route route in loadedTours)
                    {
                        try
                        {
                            _mainViewModel._tourService.AddTour(route, false);
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

        public void CreateTourStatsPdf(Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)> tourData, string filePath)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Tour Statistics Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

            gfx.DrawString("Tour Statistics Report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

            int yPoint = 60;

            foreach (var tour in tourData)
            {
                string line = $"Tour Name: {tour.Key}, Average Difficulty: {tour.Value.averageDifficulty}, Average Time: {tour.Value.averageTime} minutes, Average Distance: {tour.Value.averageDistance} km";
                gfx.DrawString(line, font, XBrushes.Black, new XRect(20, yPoint, page.Width - 40, page.Height), XStringFormats.TopLeft);
                yPoint += 20;
            }

            document.Save(filePath);
            MessageBox.Show("Tour statistics report has been saved successfully.");
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
                CreateTourStatsPdf(tourStats, saveFileDialog.FileName);
            }
        }


    }
}
