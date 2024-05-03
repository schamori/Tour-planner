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
        Tour? GetTour(string tourName);

        void Add(Tour obj);
        List<Tour> GetAllTours();
        void DeleteTour(string tourName);
        void UpdateTour(Tour obj);

        public Tour? GetTourById(Guid tourId);


    }
}
