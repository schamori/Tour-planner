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

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private DatabaseManager _dbManager;
        public MainWindow()
        {
            _dbManager = new DatabaseManager("Host=localhost;Port=5432;Database=postgresdb;Username=mpleyer;Password=admin;");

            InitializeComponent();
            this.DataContext = new MainWindowViewModel(_dbManager, new AddTourViewModel());
        }

    }
}
