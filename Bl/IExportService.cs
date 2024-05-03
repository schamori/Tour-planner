using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Bl
{
    public interface IExportService
    {
        void CreateTourStatsPdf(Dictionary<string, (double averageDifficulty, double averageTime, double averageDistance)> tourData, string filePath);

        public void CreateTourReportPdf(Tour tour, string filePath, int Popularity, bool? ChildFriendliness);

    }
}
