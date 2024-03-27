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

        public ICommand DeleteCommand { get; set; }

        public ICommand ModifyCommand { get; set; }



        public MainWindowViewModel()
        {
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
