using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner.ViewModels;
using Models;
using Bl;
using DAL;
using log4net;
using TourPlanner.Views;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    

    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        private DatabaseManager _dbManager;
        public MainWindow()
        {
            var connectionString = "Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin";

            _dbManager = new DatabaseManager(connectionString);
            ITourRepo tourRepo = new TourRepo(connectionString);
            ITourLogRepo tourLogRepo = new TourLogsRepo(connectionString);

            ITourService tourService = new TourService(tourRepo);
            ITourLogService tourLogService = new TourLogService(tourLogRepo);
            InitializeComponent();
            ToursView.TourSelected += ToursView_TourSelected;
            this.DataContext = new MainWindowViewModel(tourService, tourLogService, _dbManager, new AddTourViewModel());

            log.Info("Application is starting.");
        }


        private void ToursView_TourSelected(object sender, TourSelectedEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;
            viewModel.SelectedTourLog(e.TourId);
        }
    }
}
