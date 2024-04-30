using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace DAL
{
    public class OpenRouteServiceClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiKey;

        public OpenRouteServiceClient(string apiKey)
        {
            _apiKey = apiKey;
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("TourPlanner/1.0 (+https://moodle.technikum-wien.at/)");
        }

        public async Task<JObject> GeocodeAddress(string address)
        {
            var response = await _client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseString);
        }

        public async Task<JObject> GetRoute(double startLat, double startLng, double endLat, double endLng, string transportType)
        {
            var requestBody = new JObject(new JProperty("coordinates", new JArray(new JArray(startLng, startLat), new JArray(endLng, endLat))));
            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            var url = $"https://api.openrouteservice.org/v2/directions/{transportType}/geojson?api_key={_apiKey}";
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseString);
        }

        public async Task DownloadTileImage(double latitude, double longitude, int zoom, Guid tourId)
        {
            var tileXY = LatLongToTileXY(latitude, longitude, zoom);
            var tileUrl = $"https://tile.openstreetmap.org/{zoom}/{tileXY.x}/{tileXY.y}.png";

            // Erstellen des Dateipfades
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bilder");
            Directory.CreateDirectory(folderPath); 
            string fileName = $"tour_{tourId}_tile_{zoom}_{tileXY.x}_{tileXY.y}.png";
            string fullPath = Path.Combine(folderPath, fileName);

            var response = await _client.GetAsync(tileUrl);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fehler beim Herunterladen der Karte: " + response.ReasonPhrase);
            }

            using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
            {
                if (streamToReadFrom.Length == 0)
                {
                    throw new Exception("Heruntergeladener Stream ist leer.");
                }

                using (var streamToWriteTo = File.Open(fullPath, FileMode.Create))
                {
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }
            }

        }

        public static (int x, int y) LatLongToTileXY(double latitude, double longitude, int zoom)
        {
            double latRad = latitude * Math.PI / 180;
            double n = Math.Pow(2, zoom);
            int x = (int)((longitude + 180.0) / 360.0 * n);
            int y = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);

            return (x, y);
        }
    }
}
