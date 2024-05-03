using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public interface IRouteService
    {
        Task<Tour> CreateRouteAsync(Guid id, string name, string description, string startAddress, string endAddress, string transportType);
        void DownloadTile(double Lat, double Lng, int zoom, Guid Id);
    }
}
