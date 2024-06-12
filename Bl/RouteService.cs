using System;
using System.Threading.Tasks;
using DAL;
using Models;
using Newtonsoft.Json.Linq;
using log4net;
using Bl;
using System.IO;

namespace Bl
{
    public class RouteService : IRouteService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RouteService));
        private readonly OpenRouteServiceClient _orsClient;

        public RouteService(string apiKey)
        {
            _orsClient = new OpenRouteServiceClient(apiKey);
        }

        public async Task<Tour> CreateRouteAsync(Guid id, string name, string description, string startAddress, string endAddress, string transportType)
        {
            try
            {
                var startCoords = await _orsClient.GeocodeAddress(startAddress);
                var endCoords = await _orsClient.GeocodeAddress(endAddress);

                var startLat = (double)startCoords["features"][0]["geometry"]["coordinates"][1];
                var startLng = (double)startCoords["features"][0]["geometry"]["coordinates"][0];
                var endLat = (double)endCoords["features"][0]["geometry"]["coordinates"][1];
                var endLng = (double)endCoords["features"][0]["geometry"]["coordinates"][0];
                JObject route = await _orsClient.GetRoute(startLat, startLng, endLat, endLng, transportType);

                var feature = route["features"][0];

                var properties = feature["properties"];

                var summary = properties["summary"];

                var distance = (double)summary["distance"];
                var duration = (int)summary["duration"];

                Tour newRoute = new Tour(id, name, description, startAddress, endAddress, transportType, distance, duration, DateTime.Now);

                await CreateMap(startLat, startLng, endLat, endLng, newRoute.Id, distance);

                return newRoute;
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while creating the route.", ex);
                throw;
            }
        }

        public async Task CreateMap(double startLat, double startLng, double endLat, double endLng, Guid Id, double distance)
        {
            try
            {
                // Normalisieren der Koordinaten
                var minLat = Math.Min(startLat, endLat);
                var maxLat = Math.Max(startLat, endLat);
                var minLon = Math.Min(startLng, endLng);
                var maxLon = Math.Max(startLng, endLng);

                var mapCreator = new MapCreator(minLon, minLat, maxLon, maxLat, _orsClient);
                mapCreator.SetZoom((int)(-1.3 * Math.Log(distance) + 26));
                mapCreator.GetMarkers().Add(new GeoCoordinate(startLng, startLat));
                mapCreator.GetMarkers().Add(new GeoCoordinate(endLng, endLat));
                await mapCreator.GenerateImageAsync();
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder"); 
                Directory.CreateDirectory(folderPath);  // Stelle sicher, dass der Ordner existiert
                mapCreator.SaveImage($"..\\..\\..\\..\\Bilder\\tour_{Id}.png");
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while creating the map.", ex);
                throw;
            }
        }
    }
}
