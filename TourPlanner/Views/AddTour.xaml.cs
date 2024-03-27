using Bl;
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

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for AddTour.xaml
    /// </summary>
    public partial class AddTour : UserControl
    {
        public AddTour()
        {
            InitializeComponent();

        }

        private async void OnCreateRouteButtonClick(object sender, RoutedEventArgs e)
        {
            var routeService = new RouteService("5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0");
            // Aktualisiere die GUI mit den erhaltenen Routendaten und dem Bild
        }
    }
}
