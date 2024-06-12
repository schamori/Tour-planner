using NUnit.Framework;
using Moq;
using TourPlanner.ViewModels;
using Bl;
using DAL;
using Models;
using System.Collections.Generic;
using System.Windows;

namespace TourTests
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel _mainWindowViewModel;
        private Mock<ITourService> _mockTourService;
        private Mock<ITourLogService> _mockTourLogService;
        private Mock<IRouteService> _mockRouteService;
        private Mock<IExportService> _mockExportService;

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
        }

        [Test]
        public void TourDetailsVisibility_ShouldBeHiddenInitially()
        {
            // Assert
            Assert.AreEqual(Visibility.Hidden, _mainWindowViewModel.TourDetailsVisibility);
        }

        [Test]
        public void SetTourDetailsVisibility_ShouldUpdateVisibility()
        {
            // Act
            _mainWindowViewModel.TourDetailsVisibility = Visibility.Visible;

            // Assert
            Assert.AreEqual(Visibility.Visible, _mainWindowViewModel.TourDetailsVisibility);
        }

        // Additional tests for other properties and methods
    }
}
