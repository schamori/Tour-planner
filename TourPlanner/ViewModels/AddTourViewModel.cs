using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        public string TourName { get; set; }
        public string TourDescription { get; set; }

        public ICommand SaveTourCommand { get; private set; }

        public AddTourViewModel()
        {
            SaveTourCommand = new RelayCommand(SaveTour);
        }

        private void SaveTour(object parameter)
        {
            // Logic to save the tour using the input data
        }
    }
}
