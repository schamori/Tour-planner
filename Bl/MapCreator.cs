using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using DAL;
using Models;

namespace Bl
{
    public class MapCreator
    {
        private readonly double _minLon;
        private readonly double _minLat;
        private readonly double _maxLon;
        private readonly double _maxLat;
        private readonly OpenRouteServiceClient _client;
        private int _zoom;
        private List<GeoCoordinate> _markers;
        private Bitmap _finalImage;
        private const string DefaultMarkerFileName = "marker-red_32px.png";  // Standard-Dateiname des Marker-Bilds

        public MapCreator(double minLon, double minLat, double maxLon, double maxLat, OpenRouteServiceClient client)
        {
            _minLon = minLon;
            _minLat = minLat;
            _maxLon = maxLon;
            _maxLat = maxLat;
            _client = client;
            _markers = new List<GeoCoordinate>();
        }

        public void SetZoom(int zoom)
        {
            _zoom = zoom;
        }

        public List<GeoCoordinate> GetMarkers()
        {
            return _markers;
        }

        public async Task GenerateImageAsync()
        {
            var topLeftTile = OpenRouteServiceClient.LatLongToTileXY(_maxLat, _minLon, _zoom);
            var bottomRightTile = OpenRouteServiceClient.LatLongToTileXY(_minLat, _maxLon, _zoom);

            int tilesX = Math.Abs(bottomRightTile.x - topLeftTile.x) + 1;
            int tilesY = Math.Abs(bottomRightTile.y - topLeftTile.y) + 1;

            if (tilesX <= 0 || tilesY <= 0)
            {
                throw new InvalidOperationException($"Invalid bounding box coordinates. topLeftX: {topLeftTile.x}, bottomRightX: {bottomRightTile.x}, topLeftY: {topLeftTile.y}, bottomRightY: {bottomRightTile.y}");
            }

            _finalImage = new Bitmap(tilesX * 256, tilesY * 256);

            using (var graphics = Graphics.FromImage(_finalImage))
            {
                for (int x = topLeftTile.x; x <= bottomRightTile.x; x++)
                {
                    for (int y = topLeftTile.y; y <= bottomRightTile.y; y++)
                    {
                        using (var tileImage = await _client.FetchTileAsync(_zoom, x, y))
                        {
                            int xPos = (x - topLeftTile.x) * 256;
                            int yPos = (y - topLeftTile.y) * 256;
                            graphics.DrawImage(tileImage, xPos, yPos);
                        }
                    }
                }

                var topLeftTilePixel = new Point(topLeftTile.x * 256, topLeftTile.y * 256);

                foreach (var marker in _markers)
                {
                    using (var markerIcon = _client.GetMarkerImage(DefaultMarkerFileName))
                    {
                        var globalPos = OpenRouteServiceClient.LatLonToPixel(marker.Latitude, marker.Longitude, _zoom);
                        var relativePos = new Point(globalPos.x - topLeftTilePixel.X, globalPos.y - topLeftTilePixel.Y);
                        graphics.DrawImage(markerIcon, relativePos.X, relativePos.Y);
                    }
                }
            }
        }

        public void SaveImage(string filename)
        {
            _finalImage.Save(filename, ImageFormat.Png);
        }
    }
}
