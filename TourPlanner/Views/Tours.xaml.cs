using Models;
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
using log4net;
using TourPlanner.ViewModels;
using static TourPlanner.ViewModels.MainWindowViewModel;

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for Tours.xaml
    /// </summary>
    public partial class Tours : UserControl
    {
        public event EventHandler<TourSelectedEventArgs> TourSelected;

        public Tours()
        {
            InitializeComponent();

        }

        protected virtual void OnTourSelected(TourSelectedEventArgs e)
        {
            TourSelected.Invoke(this, e);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            Tour selectedTour = (Tour)listBox.SelectedItem; 

            if (selectedTour != null)
            {
                // Cast the DataContext to MainWindowViewModel
                var viewModel = this.DataContext as TourViewModel;

                // Ensure the cast was successful
                if (viewModel != null)
                {
                    // Call the method
                    viewModel.SelectTour(selectedTour.Id);
                }

            }
        }
    }
}
