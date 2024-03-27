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
        public string ImageUrl { get; set; }
        public JObject Routee { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public int EstimatedTime { get; set; }
        public DateTime CreationDate { get; set; }

        public Route(string name, string description, string startAddress, string endAddress, JObject route, string transportType, double distance, int estimatedTime, DateTime creationDate) {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StartAddress = startAddress;
            EndAddress = endAddress;
            Routee = route;
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            CreationDate = creationDate;
        }
    }

}
