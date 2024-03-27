using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models;

namespace Bl
{
    public class RouteService
    {
        private readonly OpenRouteServiceClient _orsClient;

        public RouteService(string apiKey)
        {
            _orsClient = new OpenRouteServiceClient(apiKey);
        }

        public async Task<Route> CreateRouteAsync(string name, string description, string startAddress, string endAddress, string transportType)
        {
            var startCoords = await _orsClient.GeocodeAddress(startAddress);
            var endCoords = await _orsClient.GeocodeAddress(endAddress);

            var startLat = (double)startCoords["features"][0]["geometry"]["coordinates"][1];
            var startLng = (double)startCoords["features"][0]["geometry"]["coordinates"][0];
            var endLat = (double)endCoords["features"][0]["geometry"]["coordinates"][1];
            var endLng = (double)endCoords["features"][0]["geometry"]["coordinates"][0];

            var route = await _orsClient.GetRoute(startLat, startLng, endLat, endLng);

            // Logik, um das Bild der Route zu bekommen, würde hier folgen
            var startTile = GetTileUrl(startLat, startLng, 10);
            var endTile = GetTileUrl(endLat, endLng, 10);

            var newRoute = new Route(name, description, startAddress, endAddress, route, transportType, 50.0, 5, DateTime.Now);
            return newRoute;
            // Erstelle eine Route-Instanz und fülle sie mit den Daten
        }

        public static (int x, int y) LatLongToTileXY(double latitude, double longitude, int zoom)
        {
            double latRad = latitude * Math.PI / 180;
            double n = Math.Pow(2, zoom);
            int x = (int)((longitude + 180.0) / 360.0 * n);
            int y = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);

            return (x, y);
        }

        public static string GetTileUrl(double latitude, double longitude, int zoom)
        {
            var tileXY = LatLongToTileXY(latitude, longitude, zoom);
            return $"https://tile.openstreetmap.org/{zoom}/{tileXY.x}/{tileXY.y}.png";
        }

        
    }

}
