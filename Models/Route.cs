using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public string ImageUrl { get; set; }
        public JObject Routee { get; set; }
        public string STile { get; set; }
        public string ETile { get; set; }

        public Route(string name, string description, string startAddress, string endAddress, JObject route) {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StartAddress = startAddress;
            EndAddress = endAddress;
            Routee = route;
        }
    }

}
