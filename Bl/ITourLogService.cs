using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public interface ITourLogService
    {
        List<TourLog> GetAllTourLogsForTour(Guid id);

        void AddTourLog(Guid id);

        Route? GetTour(string tourName);
        void DeleteTour(string tourName);
        void ModifyTour();
    }
}
