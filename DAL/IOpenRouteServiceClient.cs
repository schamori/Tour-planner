using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IOpenRouteServiceClient
    {
        Task<JObject> GeocodeAddress(string address);
        Task<JObject> GetRoute(double startLat, double startLng, double endLat, double endLng, string transportType);
        Task<Bitmap> FetchTileAsync(int zoom, int x, int y);
        Bitmap GetMarkerImage(string markerFileName);
    }
}
