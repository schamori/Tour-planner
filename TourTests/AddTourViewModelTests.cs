using NUnit.Framework;
using Moq;
using TourPlanner.ViewModels;
using Bl;
using DAL;
using Models;
using System.Collections.Generic;

namespace TourTests
{
    [TestFixture]
    public class AddTourViewModelTests
    {
        private AddTourViewModel _addTourViewModel;
        private Mock<ITourService> _mockTourService;
        private Mock<ITourLogService> _mockTourLogService;
        private Mock<IRouteService> _mockRouteService;
        private Mock<IExportService> _mockExportService;
        private MainWindowViewModel _mainWindowViewModel;

        [SetUp]
        public void SetUp()
        {
            _mockTourService = new Mock<ITourService>();
            _mockTourLogService = new Mock<ITourLogService>();
            _mockRouteService = new Mock<IRouteService>();
            _mockExportService = new Mock<IExportService>();

            _mockTourService.Setup(s => s.GetAllTours()).Returns(new List<Tour>()); // Return an empty list

            _mainWindowViewModel = new MainWindowViewModel(
                _mockTourService.Object,
                _mockTourLogService.Object,
                _mockRouteService.Object,
                _mockExportService.Object
            );

            _addTourViewModel = new AddTourViewModel(_mainWindowViewModel);
        }

        [Test]
        public void ExecuteAddTour_WhenNameIsEmpty_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Description = "Description";
            _addTourViewModel.From = "Start Location";
            _addTourViewModel.To = "End Location";
            _addTourViewModel.TransportType = "Car";

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("Name not set", _addTourViewModel.ErrorMessage);
        }

        [Test]
        public void ExecuteAddTour_WhenDescriptionIsEmpty_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Name = "New Tour";
            _addTourViewModel.From = "Start Location";
            _addTourViewModel.To = "End Location";
            _addTourViewModel.TransportType = "Car";

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("Description not set", _addTourViewModel.ErrorMessage);
        }

        [Test]
        public void ExecuteAddTour_WhenFromIsEmpty_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Name = "New Tour";
            _addTourViewModel.Description = "Description";
            _addTourViewModel.To = "End Location";
            _addTourViewModel.TransportType = "Car";

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("From location not set", _addTourViewModel.ErrorMessage);
        }

        [Test]
        public void ExecuteAddTour_WhenToIsEmpty_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Name = "New Tour";
            _addTourViewModel.Description = "Description";
            _addTourViewModel.From = "Start Location";
            _addTourViewModel.TransportType = "Car";

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("To location not set", _addTourViewModel.ErrorMessage);
        }

        [Test]
        public void ExecuteAddTour_WhenTransportTypeIsEmpty_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Name = "New Tour";
            _addTourViewModel.Description = "Description";
            _addTourViewModel.From = "Start Location";
            _addTourViewModel.To = "End Location";

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("TransportType not set", _addTourViewModel.ErrorMessage);
        }

        [Test]
        public void ExecuteAddTour_WhenTourWithSameNameExists_SetsErrorMessage()
        {
            // Arrange
            _addTourViewModel.Name = "Existing Tour";
            _addTourViewModel.Description = "Description";
            _addTourViewModel.From = "Start Location";
            _addTourViewModel.To = "End Location";
            _addTourViewModel.TransportType = "Car";

            _mockTourService.Setup(s => s.GetTour("Existing Tour")).Returns(new Tour());

            // Act
            _addTourViewModel.AddTourCommand.Execute(null);

            // Assert
            Assert.AreEqual("Name already taken", _addTourViewModel.ErrorMessage);
        }
    }
}
