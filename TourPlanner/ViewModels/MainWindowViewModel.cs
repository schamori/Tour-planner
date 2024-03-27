using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Bl;
using DAL;


namespace TourPlanner.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        public ICommand AddCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public ICommand RandGenItemCommand { get; set; }

        public ICommand RandGenLogCommand { get; set; }


        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(o =>
            {
                Console.WriteLine("Pressed");
            });
        }
    }
}
