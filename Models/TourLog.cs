using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TourLog
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string Difficulty { get; set; }
        public double TotalDistance { get; set; }
        public int TotalTime { get; set; }
        public string Rating { get; set; }
        // Foreign Key für die Zuordnung zu einer Tour
        public Guid TourId { get; set; }

        public TourLog(string comment, string difficulty, double totoldistance, int totaltime, string rating, Guid tourId) {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totoldistance;
            TotalTime = totaltime;
            Rating = rating;
            TourId = tourId;
        }

        public TourLog(Guid id, DateTime date, string comment, string difficulty, double totoldistance, int totaltime, string rating, Guid tourId)
        {
            Id = id;
            Date = date;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totoldistance;
            TotalTime = totaltime;
            Rating = rating;
            TourId = tourId;
        }
    }

}
