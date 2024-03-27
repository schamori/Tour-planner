using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace Bl
{
    public class MyNavigationService : INavigationService
    {
        private Frame _frame;

        public MyNavigationService(Frame frame)
        {
            _frame = frame;
            _frame.Navigate(new Uri("pack://application:,,,/TourPlanner;component/HomePage.xaml", UriKind.Absolute));

        }
        public void NavigateTo(string pageKey)
        {
            switch (pageKey)
            {
                case "HomePage":
                    _frame.Navigate(new Uri("TourPlanner/HomePage.xaml", UriKind.Relative));
                    break;
                case "AddTour":
                    _frame.Navigate(new Uri("pack://application:,,,/TourPlanner;component/AddTour.xaml", UriKind.Absolute));
                    break;
                default:
                    throw new ArgumentException("Unknown page key", nameof(pageKey));
            }
        }
    }
}
