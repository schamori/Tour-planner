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
    public class TourLogServiceTests
    {
        private Mock<ITourLogRepo> _mockTourLogRepo;
        private TourLogService _tourLogService;

        [SetUp]
        public void Setup()
        {
            _mockTourLogRepo = new Mock<ITourLogRepo>();
            _tourLogService = new TourLogService(_mockTourLogRepo.Object);
        }

        [Test]
        public void AddTourLog_ShouldAddTourLog()
        {
            var tourLog = new TourLog(Guid.NewGuid(), DateTime.Now, "Nice trip", "Medium", 120, 60, "5 stars", Guid.NewGuid());

            _tourLogService.AddTourLog(tourLog.Comment, tourLog.Difficulty, tourLog.TotalDistance, tourLog.TotalTime, tourLog.Rating, tourLog.TourId);

            _mockTourLogRepo.Verify(repo => repo.AddTourLog(It.Is<TourLog>(log =>
                log.Comment == tourLog.Comment &&
                log.Difficulty == tourLog.Difficulty &&
                log.TotalDistance == tourLog.TotalDistance &&
                log.TotalTime == tourLog.TotalTime &&
                log.Rating == tourLog.Rating &&
                log.TourId == tourLog.TourId)), Times.Once);
        }

        [Test]
        public void DeleteTour_ShouldDeleteTourLogs()
        {
            var tourId = Guid.NewGuid();

            _tourLogService.DeleteTour(tourId);

            _mockTourLogRepo.Verify(repo => repo.DeleteTourLog(tourId), Times.Once);
        }

        [Test]
        public void GetAllTourLogsForTour_ShouldReturnLogs()
        {
            var tourId = Guid.NewGuid();
            var tourLogs = new List<TourLog>
            {
                new TourLog(Guid.NewGuid(), DateTime.Now, "Log1", "Easy", 100, 50, "4 stars", tourId),
                new TourLog(Guid.NewGuid(), DateTime.Now, "Log2", "Medium", 200, 100, "5 stars", tourId)
            };
            _mockTourLogRepo.Setup(repo => repo.GetAllTourLogsForTour(tourId)).Returns(tourLogs);

            var result = _tourLogService.GetAllTourLogsForTour(tourId);

            Assert.AreEqual(tourLogs, result);
        }

        [Test]
        public void GetLog_ShouldReturnLog()
        {
            var logId = Guid.NewGuid();
            var tourLog = new TourLog(logId, DateTime.Now, "Nice trip", "Medium", 120, 60, "5 stars", Guid.NewGuid());
            _mockTourLogRepo.Setup(repo => repo.GetSingleLog(logId)).Returns(tourLog);

            var result = _tourLogService.GetLog(logId);

            Assert.AreEqual(tourLog, result);
        }

        [Test]
        public void UpdateLogId_ShouldUpdateLogIds()
        {
            var oldTourId = Guid.NewGuid();
            var newTourId = Guid.NewGuid();

            _tourLogService.UpdateLogId(oldTourId, newTourId);

            _mockTourLogRepo.Verify(repo => repo.UpdateLogId(oldTourId, newTourId), Times.Once);
        }
    }
}
