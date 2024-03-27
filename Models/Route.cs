using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Route
    {
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public string ImageUrl { get; set; }
        // ... Weitere Eigenschaften nach Bedarf
    }

}
