using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Bl
{
    public interface ITourService
    {
        List<Tour> GetAllTours();
        
        void AddTour(Tour route, bool update);

        Tour? GetTourById(Guid tourId);

        Tour? GetTour(string tourName);
        void DeleteTour(string tourName);
        void ModifyTour();

        void ChangeTourFavorite(Guid tourId, bool toFavorite);
    }
}
