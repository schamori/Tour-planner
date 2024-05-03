using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;


namespace Bl
{
    public class TourService : ITourService
    {
        private ITourRepo _tourRepo;
        public TourService(ITourRepo tourRepo)
        {
            _tourRepo = tourRepo;
        }
        public void AddTour(Tour route, bool update)
        {
            if (update)
                _tourRepo.UpdateTour(route);
            else
                _tourRepo.Add(route);


        }

        public void DeleteTour(string tourName)
        {
            _tourRepo.DeleteTour(tourName);
        }

        public List<Tour> GetAllTours()
        {
            return _tourRepo.GetAllTours();
        }

        public Tour? GetTour(string tourName)
        {
            return _tourRepo.GetTour(tourName);
        }

        public Tour? GetTourById(Guid tourId)
        {
            return _tourRepo.GetTourById(tourId);
        }

        public void ModifyTour()
        {
            throw new NotImplementedException();
        }
    }
}
