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
        List<Route> GetAllTours();
        
        void AddTour(Route route);

        Route? GetTour(string tourName);
        void DeleteTour(string tourName);
        void ModifyTour();
    }
}
