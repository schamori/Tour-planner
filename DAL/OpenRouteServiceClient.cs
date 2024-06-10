using log4net;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace DAL
{
    public class OpenRouteServiceClient
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenRouteServiceClient));
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiKey;

        public OpenRouteServiceClient(string apiKey)
        {
            _apiKey = apiKey;
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("TourPlanner/1.0 (+https://moodle.technikum-wien.at/)");
        }

        public async Task<JObject> GeocodeAddress(string address)
        {
            try
            {
                var response = await _client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while geocoding the address.", ex);
                throw;
            }
        }

        public async Task<JObject> GetRoute(double startLat, double startLng, double endLat, double endLng, string transportType)
        {
            try
            {
                var requestBody = new JObject(new JProperty("coordinates", new JArray(new JArray(startLng, startLat), new JArray(endLng, endLat))));
                var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
                var url = $"https://api.openrouteservice.org/v2/directions/{transportType}/geojson?api_key={_apiKey}";
                var response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while getting the route.", ex);
                throw;
            }
        }

        public async Task<Bitmap> FetchTileAsync(int zoom, int x, int y)
        {
            var tileUrl = $"https://tile.openstreetmap.org/{zoom}/{x}/{y}.png";
            var response = await _client.GetAsync(tileUrl);
            response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                return new Bitmap(stream);
            }
        }

        public Bitmap GetMarkerImage(string markerFileName)
        {
            // Erstellen Sie den absoluten Pfad zum Marker-Bild
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..\\..", "Bl\\resources");
            string absolutePath = Path.Combine(folderPath, markerFileName);

            if (!File.Exists(absolutePath))
            {
                throw new FileNotFoundException("Marker image file not found.", absolutePath);
            }

            return new Bitmap(absolutePath);
        }


        public static (int x, int y) LatLongToTileXY(double latitude, double longitude, int zoom)
        {
            double latRad = Math.PI / 180.0 * latitude;
            double n = Math.Pow(2.0, zoom);
            int x = (int)Math.Floor((longitude + 180.0) / 360.0 * n);
            int y = (int)Math.Floor((1.0 - Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);
            return (x, y);
        }

        public static (int x, int y) LatLonToPixel(double lat, double lon, int zoom)
        {
            double latRad = lat * Math.PI / 180;
            double n = Math.Pow(2.0, zoom);
            int x = (int)((lon + 180.0) / 360.0 * n * 256.0);
            int y = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n * 256.0);
            return (x, y);
        }
    }
}
