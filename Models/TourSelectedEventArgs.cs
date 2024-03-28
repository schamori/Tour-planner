using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TourSelectedEventArgs : EventArgs
    {
        public Guid TourId { get; private set; }

        public TourSelectedEventArgs(Guid tourId)
        {
            TourId = tourId;
        }
    }
}
