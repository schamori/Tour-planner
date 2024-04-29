using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TourLog
    {

        [Required]
        public Guid Id { get; set; }
        private DateTime _date;

        [Required]
        public DateTime Date
        {
            get => _date;
            set => _date = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }
        [Required]

        public string Comment { get; set; }
        [Required]

        public string Difficulty { get; set; }
        [Required]

        public double TotalDistance { get; set; }
        [Required]

        public int TotalTime { get; set; }
        [Required]

        public string Rating { get; set; }
        [Required]

        public Guid TourId { get; set; }

        public TourLog(Guid id, DateTime date, string comment, string difficulty, double totalDistance, int totalTime, string rating, Guid tourId)
        {
            Id = id;
            Date = date;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totalDistance;
            TotalTime = totalTime;
            Rating = rating;
            TourId = tourId;
        }
    }
}
