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
        }

        public async Task<JObject> GeocodeAddress(string address)
        {
            var response = await _client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0&text={Uri.EscapeDataString(address)}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseString);
        }

        public async Task<JObject> GetRoute(double startLat, double startLng, double endLat, double endLng)
        {
            var requestBody = new JObject(new JProperty("coordinates", new JArray(new JArray(startLng, startLat), new JArray(endLng, endLat))));
            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"https://api.openrouteservice.org/v2/directions/driving-car/geojson?api_key=5b3ce3597851110001cf62481e3cc9942506493089ff10a91977e5c0", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseString);
        }
    }
}
