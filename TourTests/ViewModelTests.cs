using TourPlanner.ViewModels;

namespace TourTests
{
    public class ViewModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NameProperty_SetsValue_NotifiesPropertyChange()
        {
            var vm = new MainWindowViewModel(null, null, null, null);
            var wasNotified = false;
            vm.PropertyChanged += (sender, e) => {
                if (e.PropertyName == nameof(MainWindowViewModel.Name)) wasNotified = true;
            };

            vm.Name = "New Name";

            Assert.IsTrue(wasNotified, "Setting the Name property did not raise the PropertyChanged event.");
        }
        [Test]
        public void GotToAddCommand_Executed_ChangesVisibilityProperties()
        {
            var vm = new MainWindowViewModel(null, null, null, null);
            var command = vm.GotToAddCommand as RelayCommand;

            command.Execute(null);

            Assert.AreEqual(Visibility.Hidden, vm.ToursVisibility);
            Assert.AreEqual(Visibility.Visible, vm.AddTourVisibility);
        }
        [Test]
        public void AddTourCommand_ExecutedWithEmptyName_SetsErrorMessage()
        {
            var vm = new MainWindowViewModel(null, null, null, null);
            var command = vm.AddTourCommand as RelayCommand;

            vm.Name = ""; // Simulate user not inputting name
            command.Execute(null);

            Assert.IsFalse(string.IsNullOrEmpty(vm.ErrorMessage), "ErrorMessage should be set when Name is empty.");
        }
        [Test]
        public void AddTourCommand_ExecutedSuccessfully_UpdatesToursCollection()
        {
            var tourServiceMock = Substitute.For<ITourService>();
            var newTour = new Route { Name = "Test Tour" };
            tourServiceMock.GetAllTours().Returns(new List<Route> { newTour });

            var vm = new MainWindowViewModel(tourServiceMock, null, null, null);
            vm.Name = "Test Tour";
            vm.Description = "Test Description";
            vm.From = "Location A";
            vm.To = "Location B";
            vm.TransportType = "Walking";

            var command = vm.AddTourCommand as RelayCommand;
            command.Execute(null);

            tourServiceMock.Received(1).AddTour(Arg.Any<Route>());
            Assert.AreEqual(1, vm.Tours.Count);
            Assert.AreEqual("Test Tour", vm.Tours.First().Name);
        }




    }
}