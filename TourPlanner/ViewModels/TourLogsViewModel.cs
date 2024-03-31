using Bl;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Views;
using System.Xml.Linq;



namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;

        private ObservableCollection<TourLog> _selectedTourLogs;
        public ObservableCollection<TourLog> SelectedTourLogs
        {
            get => _selectedTourLogs;
            set
            {
                if (_selectedTourLogs != value)
                {
                    _selectedTourLogs = value;
                    OnPropertyChanged(nameof(SelectedTourLogs));
                }
            }
        }

        public ICommand AddLogCommand { get; private set; }
        public ICommand CancelLogCommand { get; private set; }
        public ICommand GoToAddLogCommand { get; private set; }
        public ICommand ModifyLogCommand { get; private set; }
        public ICommand DeleteLogCommand { get; private set; }

        public TourLogsViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            AddLogCommand = new RelayCommand(DeleteLogAction);
            ModifyLogCommand = new RelayCommand(ModifyLogAction);
            DeleteLogCommand = new RelayCommand(DeleteLogAction);
            GoToAddLogCommand = new RelayCommand(o =>
            {
                _mainViewModel.LogVisibility = Visibility.Hidden;
                _mainViewModel.AddLogVisibility = Visibility.Visible;
            });
        }

        private void ModifyLogAction(object parameter)
        {
            
            TourLog tourLog = parameter as TourLog;
            _mainViewModel.AddTourLogsVM.logToModify = tourLog.Id;
            _mainViewModel.AddTourLogsVM.Comment = tourLog.Comment;
            _mainViewModel.AddTourLogsVM.Difficulty = tourLog.Difficulty;
            _mainViewModel.AddTourLogsVM.Rating = tourLog.Rating;
            _mainViewModel.AddTourLogsVM.TotalDistance = tourLog.TotalDistance.ToString();
            _mainViewModel.AddTourLogsVM.TotalTime = tourLog.TotalTime.ToString();
            _mainViewModel.LogVisibility = Visibility.Hidden;
            _mainViewModel.AddLogVisibility = Visibility.Visible;
        }


        private void DeleteLogAction(object parameter)
        {
            TourLog tourLog = parameter as TourLog;
            _mainViewModel._tourLogService.DeleteTour(tourLog.Id);
            // refresh
            _mainViewModel.TourVM.SelectTour(tourLog.TourId);
        }
    }
}

