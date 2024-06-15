using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bl;
using Models;

namespace TourTests
{
    [SetUpFixture]
    public class TestsSetup
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }

    [TestFixture]
    public class ExportServiceTests
    {
        private ExportService _exportService;
        private string _testFilePath;

        [SetUp]
        public void SetUp()
        {
            _exportService = new ExportService();
            _testFilePath = Path.Combine(Path.GetTempPath(), "test.pdf");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public void CreateTourStatsPdf_ShouldCreatePdfFile()
        {
            var tourData = new Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)>
            {
                { "Tour1", (1.5, 120, 5.0) },
                { "Tour2", (2.0, 150, 7.5) }
            };

            _exportService.CreateTourStatsPdf(tourData, _testFilePath);

            Assert.IsTrue(File.Exists(_testFilePath), "The PDF file was not created.");
        }

        [Test]
        public void CreateTourReportPdf_ShouldCreatePdfFile()
        {
            var tour = new Tour
            {
                Id = Guid.NewGuid(),
                Name = "Test Tour",
                Description = "Test Description",
                StartAddress = "Test Start",
                EndAddress = "Test End",
                TransportType = "Test Transport",
                Distance = 10.0,
                EstimatedTime = 3600,
                CreationDate = DateTime.Now,
                TourLogs = new List<TourLog>
                {
                    new TourLog(Guid.NewGuid(), DateTime.Now, "Great Tour", "Easy", 10.0, 3600, "5 stars", Guid.NewGuid())
                }
            };

            _exportService.CreateTourReportPdf(tour, _testFilePath, 5, true);

            Assert.IsTrue(File.Exists(_testFilePath), "The PDF file was not created.");
        }
    }
}
