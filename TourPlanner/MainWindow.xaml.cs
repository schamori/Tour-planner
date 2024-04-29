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
using Microsoft.EntityFrameworkCore;


namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    

    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        public MainWindow()
        {
            var connectionString = "Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            AppDbContext _context = new AppDbContext(optionsBuilder.Options);
            _context.Database.Migrate();
            _context.EnsureDatabase();
            DatabaseManager _dbManager = new DatabaseManager(connectionString);
            ITourRepo tourRepo = new TourRepo(connectionString);
            ITourLogRepo tourLogRepo = new TourLogsRepo(_context);

            ITourService tourService = new TourService(tourRepo);
            ITourLogService tourLogService = new TourLogService(tourLogRepo);
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(tourService, tourLogService);

            log.Info("Application is starting.");
        }



    }
}
