using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GeoCoordinate
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public GeoCoordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
