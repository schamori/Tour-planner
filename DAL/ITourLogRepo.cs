using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ITourLogRepo
    {
        void AddTourLog(TourLog obj);
        List<TourLog> GetAllTourLogsForTour(Guid id);
        void DeleteTourLog(Guid id);
        TourLog? GetSingleLog(Guid id);
        void UpdateTourLog();
        void UpdateLogId(Guid oldTourId, Guid tourId);
    }
}
