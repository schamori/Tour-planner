using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TourPlanner.ViewModels;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for Tours.xaml
    /// </summary>
    public partial class Tours : UserControl
    {
        // Definiere einen EventHandler mit den TourSelectedEventArgs
        private static readonly ILog log = LogManager.GetLogger(typeof(Tours));
        public event EventHandler<TourSelectedEventArgs> TourSelected;

        public Tours()
        {
            InitializeComponent();

        }

        // Methode zum Auslösen des Events
        protected virtual void OnTourSelected(TourSelectedEventArgs e)
        {
            log.Info("Tour selected: " + e.TourId);
            TourSelected?.Invoke(this, e);
        }

        // Ein Beispiel, wie du das Event auslösen könntest (z.B. bei Auswahl in einer ListBox)
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedTour = listBox.SelectedItem as Route; // Tour ist dein Modell oder Datenklasse
            if (selectedTour != null)
            {
                OnTourSelected(new TourSelectedEventArgs(selectedTour.Id));
            }
        }
    }
}
