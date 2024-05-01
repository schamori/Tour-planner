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
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Windows;
using Models;

namespace TourPlanner.ViewModels
{
    public class DropdownModelView
    {
        public readonly MainWindowViewModel _mainViewModel;

        public ICommand ImportCommand;

        public ICommand ExportCommand;
        public ICommand TourReportCommand;
        public ICommand SummarizeReportCommand { get; set; }


        private void ExecuteImportCommand()
        {

        }

        private void ExecuteExportCommand()
        {

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
        public DropdownModelView(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ImportCommand = new RelayCommand(o => { ExecuteImportCommand(); });
            ExportCommand = new RelayCommand(o => { ExecuteExportCommand(); });
            SummarizeReportCommand = new RelayCommand(o => { ExecuteSummarizeReportCommand(); });
        }


    }
}
