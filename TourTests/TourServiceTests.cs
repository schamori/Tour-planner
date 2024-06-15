using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Bl;
using DAL;
using Models;

namespace TourTests
{
    [TestFixture]
    public class TourServiceTests
    {
        private Mock<ITourRepo> _mockTourRepo;
        private TourService _tourService;

        [SetUp]
        public void Setup()
        {
            _mockTourRepo = new Mock<ITourRepo>();
            _tourService = new TourService(_mockTourRepo.Object);
        }

        [Test]
        public void AddTour_ShouldAddTour()
        {
            var tour = new Tour { Id = Guid.NewGuid(), Name = "Tour1" };

            _tourService.AddTour(tour, false);

            _mockTourRepo.Verify(repo => repo.Add(It.Is<Tour>(t => t.Id == tour.Id && t.Name == tour.Name)), Times.Once);
        }

        [Test]
        public void ChangeTourFavorite_ShouldChangeFavoriteStatus()
        {
            var tourId = Guid.NewGuid();
            var toFavorite = true;

            _tourService.ChangeTourFavorite(tourId, toFavorite);

            _mockTourRepo.Verify(repo => repo.ChangeTourFavorite(tourId, toFavorite), Times.Once);
        }

        [Test]
        public void DeleteTour_ShouldDeleteTour()
        {
            var tourName = "Tour1";

            _tourService.DeleteTour(tourName);

            _mockTourRepo.Verify(repo => repo.DeleteTour(tourName), Times.Once);
        }

        [Test]
        public void GetAllTours_ShouldReturnAllTours()
        {
            var tours = new List<Tour> { new Tour { Id = Guid.NewGuid(), Name = "Tour1" } };
            _mockTourRepo.Setup(repo => repo.GetAllTours()).Returns(tours);

            var result = _tourService.GetAllTours();

            Assert.AreEqual(tours, result);
        }

        [Test]
        public void GetTour_ShouldReturnTour()
        {
            var tourName = "Tour1";
            var tour = new Tour { Id = Guid.NewGuid(), Name = tourName };
            _mockTourRepo.Setup(repo => repo.GetTour(tourName)).Returns(tour);

            var result = _tourService.GetTour(tourName);

            Assert.AreEqual(tour, result);
        }

        [Test]
        public void GetTourById_ShouldReturnTour()
        {
            var tourId = Guid.NewGuid();
            var tour = new Tour { Id = tourId, Name = "Tour1" };
            _mockTourRepo.Setup(repo => repo.GetTourById(tourId)).Returns(tour);

            var result = _tourService.GetTourById(tourId);

            Assert.AreEqual(tour, result);
        }
    }
}
