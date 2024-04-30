using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private string _mapImageUrl;
        public string MapImageUrl
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
            string searchPattern = $"tour_{tourId}_tile_*.png";  // Der Suchpattern entspricht dem Speichermuster
            var files = Directory.GetFiles(folderPath, searchPattern);
            if (files.Length > 0)
            {
                // Der 'file:///' ist notwendig, damit WPF den lokalen Dateipfad als URI interpretieren kann
                MapImageUrl = "file:///" + files[0].Replace("\\", "/");
            }
            else
            {
                // Setze ein Standardbild oder eine Fehlermeldung
                MapImageUrl = "path_to_default_image.png";
            }
        }
    }
}
