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

        public async Task<Route> CreateRouteAsync(string startAddress, string endAddress)
        {
            var startCoords = await _orsClient.GeocodeAddress(startAddress);
            var endCoords = await _orsClient.GeocodeAddress(endAddress);

            var startLat = (double)startCoords["features"][0]["geometry"]["coordinates"][1];
            var startLng = (double)startCoords["features"][0]["geometry"]["coordinates"][0];
            var endLat = (double)endCoords["features"][0]["geometry"]["coordinates"][1];
            var endLng = (double)endCoords["features"][0]["geometry"]["coordinates"][0];

            var route = await _orsClient.GetRoute(startLat, startLng, endLat, endLng);

            // Logik, um das Bild der Route zu bekommen, würde hier folgen
            // ...

            // Erstelle eine Route-Instanz und fülle sie mit den Daten
            return new Route
            {
                // ... Setze Eigenschaften der Route mit den erhaltenen Daten
            };
        }
    }

}
