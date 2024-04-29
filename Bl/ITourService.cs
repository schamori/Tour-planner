﻿using System;
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
        
        Route AddTour(Route route);

        Route? GetTourById(Guid tourId);

        Route? GetTour(string tourName);
        void DeleteTour(string tourName);
        void ModifyTour();
    }
}
