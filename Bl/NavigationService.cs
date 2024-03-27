using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bl
{
    public class MyNavigationService : INavigationService
    {
        private Frame _frame;

        public MyNavigationService(Frame frame)
        {
            _frame = frame;
        }
        public void NavigateTo(string pageKey)
        {
            switch (pageKey)
            {
                case "HomePage":
                    _frame.Navigate(new Uri("TourPlanner/HomePage.xaml", UriKind.Relative));
                    break;
                case "AddTour":
                    _frame.Navigate(new Uri("TourPlanner\AddTour.xaml", UriKind.Relative));
                    break;
                default:
                    throw new ArgumentException("Unknown page key", nameof(pageKey));
            }
        }
    }
}
