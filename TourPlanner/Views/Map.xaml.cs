using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
        }

        private void Image_OnLoaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Bild erfolgreich geladen.");
        }

        private void Image_OnFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Console.WriteLine("Fehler beim Laden des Bildes: " + e.ErrorException.Message);
        }
    }

}
