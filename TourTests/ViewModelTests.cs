using TourPlanner.ViewModels;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Moq;
using System.Windows;
using Bl;
using Models;

namespace TourTests
{
    public class ViewModelTests
    {

        public Mock<MainWindowViewModel> mockViewModel;
        [SetUp]
        public void Setup()
        {
            // Assuming MainWindowViewModel has a constructor that takes an IMyService
            var tourServiceMock = new Mock<ITourService>();
            var logServiceMock = new Mock<ITourLogService>();
            tourServiceMock.Setup(service => service.GetAllTours()).Returns(new List<Route>());


            mockViewModel = new Mock<MainWindowViewModel>(tourServiceMock.Object, logServiceMock.Object);

        }

        [Test]
        public void NameProperty_SetsValue_NotifiesPropertyChange()
        {
            var vm = new AddTourViewModel(mockViewModel.Object);
            var wasNotified = false;
            vm.PropertyChanged += (sender, e) => {
                if (e.PropertyName == nameof(AddTourViewModel.Name)) wasNotified = true;
            };

            vm.Name = "New Name";

            Assert.IsTrue(wasNotified, "Setting the Name property did not raise the PropertyChanged event.");
        }
        [Test]
        public void AddTourCommand_ExecutedWithEmptyName_SetsErrorMessage()
        {
            var vm = new AddTourViewModel(mockViewModel.Object);

            var command = vm.AddTourCommand as RelayCommand;

            vm.Name = "";

            command.Execute(null);

            Assert.That(vm.ErrorMessage, Is.EqualTo("Name not set"));
        }

        [Test]
        public void AddTourCommand_ExecutedWithEmptyFrom_SetsErrorMessage()
        {
            var vm = new AddTourViewModel(mockViewModel.Object);

            var command = vm.AddTourCommand as RelayCommand;
            vm.Name = "Test";
            vm.Description = "Test";

            vm.From = "";

            command.Execute(null);

            Assert.That(vm.ErrorMessage, Is.EqualTo("From location not set"));
        }

        [Test]
        public void AddTourCommand_ExecutedWithEmptyTo_SetsErrorMessage()
        {
            var vm = new AddTourViewModel(mockViewModel.Object);

            var command = vm.AddTourCommand as RelayCommand;

            vm.Name = "Test";

            vm.Description = "Test";
            vm.From = "Vienna";

            vm.To = "";

            command.Execute(null);

            Assert.That(vm.ErrorMessage, Is.EqualTo("To location not set"));
        }




    }
}