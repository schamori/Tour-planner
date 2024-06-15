using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DAL;
using Models;
using Bl;

namespace TourTests
{
    [TestFixture]
    public class MapCreatorTests
    {
        private Mock<OpenRouteServiceClient> _mockClient;
        private MapCreator _mapCreator;
        private const string DefaultMarkerFileName = "marker-red_32px.png";

        [SetUp]
        public void SetUp()
        {
            _mockClient = new Mock<OpenRouteServiceClient>("test_api_key");
            _mapCreator = new MapCreator(16.321, 48.137, 16.324, 48.141, _mockClient.Object);
            _mapCreator.SetZoom(17);
        }

        [Test]
        public void SetZoom_ShouldSetZoomLevel()
        {
            _mapCreator.SetZoom(18);
            Assert.AreEqual(18, _mapCreator.GetType().GetField("_zoom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_mapCreator));
        }

        [Test]
        public void GetMarkers_ShouldReturnMarkers()
        {
            var markers = _mapCreator.GetMarkers();
            Assert.IsNotNull(markers);
            Assert.IsInstanceOf<List<GeoCoordinate>>(markers);
        }

        [Test]
        public void SaveImage_ShouldSaveImage()
        {
            var bitmap = new Bitmap(256, 256);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.Dispose();

            _mapCreator.GetType().GetField("_finalImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_mapCreator, bitmap);

            var tempFilePath = Path.GetTempFileName();
            _mapCreator.SaveImage(tempFilePath);

            Assert.IsTrue(File.Exists(tempFilePath));

            var savedImage = new Bitmap(tempFilePath);
            Assert.IsNotNull(savedImage);

            savedImage.Dispose();  // Disposing the image to release the file handle

            File.Delete(tempFilePath);
        }

    }
}
