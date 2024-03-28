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
        public void AddTour(Route route)
        {

           _tourRepo.Add(route);

        }

        public void DeleteTour(string tourName)
        {
            _tourRepo.DeleteTour(tourName);
        }

        public List<Route> GetAllTours()
        {
            return _tourRepo.GetAllTours();
        }

        public Route? GetTour(string tourName)
        {
            return _tourRepo.GetTour(tourName);
        }

        public void ModifyTour()
        {
            throw new NotImplementedException();
        }
    }
}
