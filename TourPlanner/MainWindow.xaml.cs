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
using Bl;
using DAL;
using log4net;

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

            ITourService tourService = new TourService(tourRepo);
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(tourService, _dbManager, new AddTourViewModel());

            log.Info("Application is starting.");
        }

    }
}
