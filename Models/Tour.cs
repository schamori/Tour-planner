using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Models
{
    [Serializable]
    public class Tour : ISerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        private string _description;
        public string Description
        {
            get => ModelHelper.WrapText(_description);
            set => _description = value;
        }


        private string _startAddress;
        public string StartAddress
        {
            get => ModelHelper.WrapText(_startAddress);
            set => _startAddress = value;
        }

        private string _endAddress;
        public string EndAddress
        {
            get => ModelHelper.WrapText(_endAddress);
            set => _endAddress = value;
        }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public int EstimatedTime { get; set; }
        private DateTime _date;

        public DateTime CreationDate
        {
            get => _date;
            set => _date = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        private ICollection<TourLog> _tourLogs;
        public ICollection<TourLog> TourLogs
        {
            get { return _tourLogs; }
            set { _tourLogs = value; }
        }

        public bool Favorite { get; set; }
        // Default constructor needed for serialization
        public Tour() { }

        // Primary constructor
        public Tour(Guid id, string name, string description, string startAddress, string endAddress, string transportType, double distance, int estimatedTime, DateTime creationDate)
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
            Favorite = false;
        }

        // Serialization constructor
        protected Tour(SerializationInfo info, StreamingContext context)
        {
            Id = (Guid)info.GetValue("Id", typeof(Guid));
            Name = info.GetString("Name");
            Description = info.GetString("Description");
            StartAddress = info.GetString("StartAddress");
            EndAddress = info.GetString("EndAddress");
            TransportType = info.GetString("TransportType");
            Distance = info.GetDouble("Distance");
            EstimatedTime = info.GetInt32("EstimatedTime");
            CreationDate = info.GetDateTime("CreationDate");
string tourLogsJson = info.GetString("TourLogs");
    if (!string.IsNullOrEmpty(tourLogsJson))
    {
        TourLogs = JsonConvert.DeserializeObject<ICollection<TourLog>>(tourLogsJson);
    }        }

        // Implementing GetObjectData for ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
            info.AddValue("StartAddress", StartAddress);
            info.AddValue("EndAddress", EndAddress);
            info.AddValue("TransportType", TransportType);
            info.AddValue("Distance", Distance);
            info.AddValue("EstimatedTime", EstimatedTime);
            info.AddValue("CreationDate", CreationDate);
            info.AddValue("TourLogs", JsonConvert.SerializeObject(TourLogs), typeof(string)); // Serialize TourLogs as JSON string
        }

    }
}
