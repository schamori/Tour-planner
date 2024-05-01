﻿using System;
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

        public void Add(Route obj)
        {
            var tour = _context.Routes.FirstOrDefault(t => t.Name == obj.Name || t.Id == obj.Id);
            if (tour != null)
                throw new RouteAlreadyExistsException();

            _context.Routes.Add(obj);
            _context.SaveChanges();
        }

        public void DeleteTour(string tourName)
        {
            var tour = _context.Routes.FirstOrDefault(t => t.Name == tourName);
            if (tour != null)
            {
                _context.Routes.Remove(tour);
                _context.SaveChanges();
            }
        }

        public List<Route> GetAllTours()
        {
            return _context.Routes.ToList();
        }

        public Route? GetTour(string tourName)
        {
            return _context.Routes.FirstOrDefault(t => t.Name == tourName);
        }

        public Route? GetTourById(Guid tourId)
        {
            return _context.Routes.FirstOrDefault(t => t.Id == tourId);
        }

        public void UpdateTour(Route obj)
        {
            var tour = _context.Routes.FirstOrDefault(t => t.Id == obj.Id);
            if (tour == null)
                throw new RouteNotFoundException();

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

