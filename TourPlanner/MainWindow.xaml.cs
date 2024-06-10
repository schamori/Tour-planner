using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.ViewModels;
using Models;
using Bl;
using DAL;
using log4net;
using TourPlanner.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;


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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "..\\..\\..", "appsettings.json");
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(folderPath, false, true) // add as content / copy-always
                .Build();

            var connectionString = config["ConnectionStrings:DefaultConnection"];
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            AppDbContext _context = new AppDbContext(optionsBuilder.Options);
            //_context.Database.Migrate();
            _context.EnsureDatabase();
            ITourRepo tourRepo = new TourRepo(_context);
            ITourLogRepo tourLogRepo = new TourLogsRepo(_context);

            ITourService tourService = new TourService(tourRepo);
            ITourLogService tourLogService = new TourLogService(tourLogRepo);

            var apiKey = config["OpenRouteService:ApiKey"];
            IRouteService routeService = new RouteService(apiKey);

            IExportService exportService = new ExportService();
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(tourService, tourLogService, routeService, exportService);

            log.Info("Application is starting.");
        }
    }
}
