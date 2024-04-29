using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace DAL
{
    public class TourRepo : ITourRepo
    {
        private readonly AppDbContext _context;

        public TourRepo(AppDbContext context)
        {
            _context = context;
        }

        // Add a Tour
        public void Add(Route obj)
        {
            _context.Routes.Add(obj);
            _context.SaveChanges();
        }

        // Delete a Tour by name
        public void DeleteTour(string tourName)
        {
            var tour = _context.Routes.FirstOrDefault(t => t.Name == tourName);
            if (tour != null)
            {
                _context.Routes.Remove(tour);
                _context.SaveChanges();
            }
        }

        // Get all Tours
        public List<Route> GetAllTours()
        {
            return _context.Routes.ToList();
        }

        // Get a Tour by name
        public Route? GetTour(string tourName)
        {
            return _context.Routes.FirstOrDefault(t => t.Name == tourName);
        }

        // Get a Tour by ID
        public Route? GetTourById(Guid tourId)
        {
            return _context.Routes.FirstOrDefault(t => t.Id == tourId);
        }

        // Update a Tour
        public void UpdateTour(Route obj)
        {
            var tour = _context.Routes.FirstOrDefault(t => t.Id == obj.Id);
            if (tour != null)
            {
                tour.Name = obj.Name;
                tour.Description = obj.Description;
                tour.Distance = obj.Distance;
                tour.CreationDate = obj.CreationDate;
                tour.EstimatedTime = obj.EstimatedTime;
                tour.StartAddress = obj.StartAddress;
                tour.EndAddress = obj.EndAddress;
                tour.TransportType = obj.TransportType;

                _context.SaveChanges();
            }
        }
    }
}

