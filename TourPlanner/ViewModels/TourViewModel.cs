using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TourPlanner.ViewModels
{
    public class TourViewModel: ViewModelBase
    {
        public class Tour
        {
            public string Name { get; set; }
        }

        public ObservableCollection<Tour> Tours { get; set; }


        public TourViewModel()
        {
            Tours = new ObservableCollection<Tour>
        {
            new Tour { Name = "Wienerwald" },
            new Tour { Name = "Dopplerhütte" },
            new Tour { Name = "FiglWarte" },
            new Tour { Name = "Dorfrunde" }
        };
        }
    }

    

}
