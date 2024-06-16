using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public interface IMapCreator
    {
        List<GeoCoordinate> GetMarkers();
        void SetZoom(int zoom);
        Task GenerateImageAsync();
        void SaveImage(string filename);
    }
}
