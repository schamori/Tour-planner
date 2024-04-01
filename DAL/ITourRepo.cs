using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace DAL
{
    public interface ITourRepo
    {
        Route? GetTour(string tourName);

        Route Add(Route obj);
        List<Route> GetAllTours();
        void DeleteTour(string tourName);
        void UpdateTour(Route obj);

        public Route? GetTourById(Guid tourId);


    }
}
