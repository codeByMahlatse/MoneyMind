using FinalApp.Services;
using FinalApp.Models;

namespace FinalApp
    
{
    public partial class App : Application
    {
        public App(LoginPage login)
        {
            InitializeComponent();

            // Let DI handle the page creation
            MainPage = new AppShell(login);
        }

        private static DatabaseService _databaseService;
        public static DatabaseService DatabaseService
        {
            get
            {
                if (_databaseService == null)
                    _databaseService = new DatabaseService();

                return _databaseService;
            }
        }

    }

}