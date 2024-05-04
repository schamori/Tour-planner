using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using DAL;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows.Media.Imaging;


namespace Bl
{
    public class ExportService: IExportService
    {

        private int yPoint = 60;
        public void CreateTourStatsPdf(Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)> tourData, string filePath)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Tour Statistics Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

            gfx.DrawString("Tour Statistics Report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);


            foreach (var tour in tourData)
            {
                string line = $"Tour Name: {tour.Key}, Average Difficulty: {tour.Value.averageDifficulty}, Average Time: {tour.Value.averageTime} minutes, Average Distance: {tour.Value.averageDistance} km";
                gfx.DrawString(line, font, XBrushes.Black, new XRect(20, yPoint, page.Width - 40, page.Height), XStringFormats.TopLeft);
                yPoint += 20;
            }

            document.Save(filePath);
            yPoint = 60;

        }

        private XImage? GetImageForTourid(Guid tourId) { 
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder");
            string searchPattern = $"tour_{tourId}_tile_*.png";  // Der Suchpattern entspricht dem Speichermuster
            var files = Directory.GetFiles(folderPath, searchPattern);

            if (files.Length > 0)
            {
            var filePath = files[0];
            XImage tourImage = XImage.FromFile(filePath);

            return tourImage;
            }
            return null;
        }

        private void DrawStringWrapped(string text, int wrapWidth, XFont font, XBrush brush, XRect rect, XStringFormat format, XGraphics gfx)
        {
            string[] words = text.Split(' ');
            string line = "";
            foreach (var word in words)
            {
                XSize size = gfx.MeasureString(line + word, font);
                if (size.Width > wrapWidth && !string.IsNullOrEmpty(line))
                {
                    gfx.DrawString(line, font, brush, rect, format);
                    line = word + " ";
                    rect.Y += 20; // Increment Y position for next line
                    yPoint += 20;
                }
                else
                {
                    line += word + " ";
                }
            }
            if (!string.IsNullOrEmpty(line))
            {
                gfx.DrawString(line, font, brush, rect, format);
            }
        }

        public void CreateTourReportPdf(Tour tour, string filePath, int Popularity, bool? ChildFriendliness)
        {

            var image = GetImageForTourid(tour.Id);

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Tour  Report";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Arial", 12, XFontStyle.BoldItalic);
            XFont font = new XFont("Arial", 10, XFontStyle.Regular);

            gfx.DrawString("Tour Report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Tour", titleFont, XBrushes.Black, new XRect(20, 40, page.Width, page.Height), XStringFormats.TopLeft);

            if (image != null)
            {
                gfx.DrawImage(image, 400, 100, image.PixelWidth / 2, image.PixelHeight / 2);
            }



            int wrapWidth = 350;

            


            yPoint += 40; // Adjust spacing for next section

            // Drawing tour details using wrapped text
            DrawStringWrapped($"Tour Name: {tour.Name}", wrapWidth,font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Description: {tour.Description}", wrapWidth, font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Start Address: {tour.StartAddress}", wrapWidth, font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"End Address: {tour.EndAddress}", wrapWidth, font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Transport Type: {tour.TransportType}", wrapWidth,font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Distance: {tour.Distance} m", wrapWidth,font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Estimated Time: {tour.EstimatedTime} seconds", wrapWidth, font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Creation Date: {tour.CreationDate.ToShortDateString()}", wrapWidth, font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Popularity: {Popularity}", wrapWidth,font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
            yPoint += 20;

            DrawStringWrapped($"Child Friendliness: {ChildFriendliness}", wrapWidth,font, XBrushes.Black, new XRect(20, yPoint, wrapWidth, page.Height - yPoint), XStringFormats.TopLeft, gfx);
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
            yPoint = 60;
        }


    }
}
