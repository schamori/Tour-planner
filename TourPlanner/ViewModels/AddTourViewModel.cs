using System;
using System.Windows.Input;
using System.Windows;
using Bl;
using Models;
// Ensure you import necessary namespaces

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        public readonly MainWindowViewModel _mainViewModel;
        private string _name = "";
        private string _description = "";
        private string _transportType = "";
        private string _from = "";
        private string _to = "";
        private string _errorMessage = "";
        private Guid _id;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string TransportType
        {
            get => _transportType;
            set
            {
                if (_transportType != value)
                {
                    _transportType = value;
                    OnPropertyChanged(nameof(TransportType));
                }
            }
        }
        public string From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged(nameof(From));
                }
            }
        }
        public string To
        {
            get => _to;
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged(nameof(To));
                }
            }
        }

        public ICommand AddTourCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public string nameToModify = "";


        public AddTourViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddTourCommand = new RelayCommand(ExecuteAddTour);
            GoBackCommand = new RelayCommand(o => { ExecuteGoBack(); });
        }

        private async void CreateNewTour(bool update, Guid oldTourId)
        {
            var routeService = new RouteService("5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0");
            Route route;

            try
            {
                route = await routeService.CreateRouteAsync(Name, Description, From, To, TransportType);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            catch (System.ArgumentNullException)
            {
                ErrorMessage = "Start or End Location not found";
                return;
            }
            _mainViewModel._tourService.AddTour(route);
            _mainViewModel.ToursVisibility = Visibility.Visible;
            _mainViewModel.AddTourVisibility = Visibility.Hidden;
            ErrorMessage = "";
            Name = "";
            Description = "";
            From = "";
            To = "";
            TransportType = "";
            ExecuteGoBack();
            if(update == true)
            {
                _mainViewModel._tourLogService.UpdateLogId(oldTourId, route.Id);
            }
            _mainViewModel.TourVM.LoadAllTours();
        }
        private void ExecuteAddTour(object paramters)
        {
            bool update = false;
            if (nameToModify != "")
            {
                _mainViewModel._tourService.DeleteTour(nameToModify);
                update = true;
            }
            if (Name == "")
            {
                ErrorMessage = "Name not set";
            }
            else if (_mainViewModel._tourService.GetTour(Name) != null)
            {
                ErrorMessage = "Name already taken";
            }
            else if (Description == "")
            {
                ErrorMessage = "Description not set";
            }
            else if (From == "")
            {
                ErrorMessage = "From location not set";
            }
            else if (To == "")
            {
                ErrorMessage = "To location not set";
            }
            else if (TransportType == "")
            {
                ErrorMessage = "TransportType not set";
            }
            else
            {
                CreateNewTour(update, Id);
            }
        }

        private void ExecuteGoBack()
        {
            _mainViewModel.ToursVisibility = Visibility.Visible;
            _mainViewModel.AddTourVisibility = Visibility.Hidden;
            ResetState();
        }

        private void ResetState()
        {
            Name = string.Empty;
            Description = string.Empty;
            TransportType = string.Empty;
            From = string.Empty;
            To = string.Empty;
            ErrorMessage = string.Empty;
            // Notify the UI that all properties have been reset
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(TransportType));
            OnPropertyChanged(nameof(From));
            OnPropertyChanged(nameof(To));
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
}
