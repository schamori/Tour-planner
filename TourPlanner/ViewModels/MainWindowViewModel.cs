using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Bl;


namespace TourPlanner.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        public ICommand GotToAddCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ModifyCommand { get; set; }

        private INavigationService _navigationService;

        public MainWindowViewModel()
        {
            GotToAddCommand = new RelayCommand(o =>
            {
                ;
            });
            AddCommand = new RelayCommand(o =>
            {
            });
            DeleteCommand = new RelayCommand(o =>
            {
            });
            ModifyCommand = new RelayCommand(o =>
            {
            });
        }
    }
}
