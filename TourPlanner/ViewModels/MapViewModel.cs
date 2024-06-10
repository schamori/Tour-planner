using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TourPlanner.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private BitmapImage _mapImageUrl;
        public BitmapImage MapImageUrl
        {
            get => _mapImageUrl;
            set
            {
                if (_mapImageUrl != value)
                {
                    _mapImageUrl = value;
                    OnPropertyChanged(nameof(MapImageUrl));
                }
            }
        }

        // Diese Methode lädt das Bild der ausgewählten Tour
        public void LoadMapImage(Guid tourId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder");
            string searchPattern = $"tour_{tourId}.png";  // Der Suchpattern entspricht dem Speichermuster
            var files = Directory.GetFiles(folderPath, searchPattern);

            if (files.Length > 0)
            {
                var filePath = files[0];
                var image = new BitmapImage(); //BitmapCacheOption um Bild vollständig in Speicher zu laden

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }

                MapImageUrl = image;
            }
            else
            {
                // Setze ein Standardbild oder eine Fehlermeldung
                MapImageUrl = new BitmapImage(new Uri("path_to_default_image.png", UriKind.RelativeOrAbsolute));
            }
        }

        public void DeleteMapImage(Guid tourId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder");
            string searchPattern = $"tour_{tourId}.png";  // Der Suchpattern entspricht dem Speichermuster
            var files = Directory.GetFiles(folderPath, searchPattern);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        public void ModifyMapImage(Guid oldTourId, Guid newTourId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder");
            string searchPattern = $"tour_{oldTourId}.png";  // Der Suchpattern entspricht dem Speichermuster
            var files = Directory.GetFiles(folderPath, searchPattern);
            foreach (var oldFilePath in files)
            {
                var oldFileName = Path.GetFileName(oldFilePath);
                var newFileName = oldFileName.Replace(oldTourId.ToString(), newTourId.ToString());
                var newFilePath = Path.Combine(folderPath, newFileName);

                // Umbenennen der Datei
                File.Move(oldFilePath, newFilePath);
            }
        }
    }
}
