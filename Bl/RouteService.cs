using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models;
using Newtonsoft.Json.Linq;

namespace Bl
{
    public class RouteService : IRouteService
    {
        private readonly OpenRouteServiceClient _orsClient;

        public RouteService(string apiKey)
        {
            _orsClient = new OpenRouteServiceClient(apiKey);
        }

        public async Task<Tour> CreateRouteAsync(Guid id, string name, string description, string startAddress, string endAddress, string transportType)
        {
            var startCoords = await _orsClient.GeocodeAddress(startAddress);
            var endCoords = await _orsClient.GeocodeAddress(endAddress);

            var startLat = (double)startCoords["features"][0]["geometry"]["coordinates"][1];
            var startLng = (double)startCoords["features"][0]["geometry"]["coordinates"][0];
            var endLat = (double)endCoords["features"][0]["geometry"]["coordinates"][1];
            var endLng = (double)endCoords["features"][0]["geometry"]["coordinates"][0];
            JObject route = await _orsClient.GetRoute(startLat, startLng, endLat, endLng, transportType);

            // Zugriff auf das erste Feature-Objekt
            var feature = route["features"][0];

            // Zugriff auf die Properties
            var properties = feature["properties"];

            // Zugriff auf Summary
            var summary = properties["summary"];

            // Extrahiere Distance und Duration
            var distance = (double)summary["distance"];
            var duration = (int)summary["duration"];

            Tour newRoute = new Tour(id, name, description, startAddress, endAddress, transportType, distance, duration, DateTime.Now);

            // Logik, um das Bild der Route zu bekommen, würde hier folgen
            DownloadTile(startLat, startLng, 17, newRoute.Id);
            DownloadTile(endLat, endLng, 17, newRoute.Id);
            return newRoute;
            // Erstelle eine Route-Instanz und fülle sie mit den Daten
        }

        public async void DownloadTile(double Lat, double Lng, int zoom, Guid Id)
        {
            await _orsClient.DownloadTileImage(Lat, Lng, zoom, Id);
        }

    }

}
