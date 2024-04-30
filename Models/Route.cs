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
        //public string ImageUrl { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public int EstimatedTime { get; set; }


        public int Popularity { get; set; }


        private DateTime _date;
        public ICollection<TourLog> TourLogs { get; set; }

        public DateTime CreationDate
        {
            get => _date;
            set => _date = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        public Route(Guid id, string name, string description, string startAddress, string endAddress, string transportType, double distance, int estimatedTime, DateTime creationDate)
        {
            Id = id;
            Name = name;
            Description = description;
            StartAddress = startAddress;
            EndAddress = endAddress;
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            CreationDate = creationDate;
            Popularity = TourLogs?.Count ?? 0;
        }



        
    }

}
