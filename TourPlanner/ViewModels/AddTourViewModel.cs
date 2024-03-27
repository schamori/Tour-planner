using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    // Properties bound to your input fields
    public string TourName { get; set; }
    public string TourDescription { get; set; }
    // Other properties...

    public AddTourViewModel()
    {
        SaveTourCommand = new RelayCommand(SaveTour);
    }

    private void SaveTour(object parameter)
    {
        // Logic to save the tour using the input data
    }
}
