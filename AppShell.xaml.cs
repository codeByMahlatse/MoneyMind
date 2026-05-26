namespace FinalApp
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();

            // Set the initial page
            CurrentItem = loginPage;

            // Register your routes if needed
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(dashboard), typeof(dashboard));
            Routing.RegisterRoute(nameof(BudgetPlannerPage), typeof(BudgetPlannerPage));
            Routing.RegisterRoute(nameof(SavingGoalsPage), typeof(SavingGoalsPage));
            Routing.RegisterRoute(nameof(DebtManagementPage), typeof(DebtManagementPage));
            Routing.RegisterRoute(nameof(FinancialReportPage), typeof(FinancialReportPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
        }
    }
}