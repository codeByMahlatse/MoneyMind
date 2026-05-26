using System.Linq;
using FinalApp.ViewModels;

namespace FinalApp;

public partial class dashboard : ContentPage
{
    private readonly string loggedInUserEmail;
    private readonly DashboardViewModel viewModel;

    public dashboard(string userEmail)
    {
        InitializeComponent();
        loggedInUserEmail = userEmail;

        viewModel = new DashboardViewModel(loggedInUserEmail); // Pass user email to ViewModel
        BindingContext = new DashboardViewModel(loggedInUserEmail);

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        viewModel.ResetFinancialData(); // Always reset first
       // _ = viewModel.LoadBudgetDataAsync(); // Reload data for logged-in user
    }

    private async void OnTileSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as string;
        if (selected == null) return;

        try
        {
            switch (selected)
            {
                case "💰 Budget Planner":
                    await Navigation.PushAsync(new BudgetPlannerPage());
                    break;
                case "📊 Financial Report":
                    await Navigation.PushAsync(new FinancialReportPage());
                    break;
                case "🎯 Saving Goal Tracker":
                    await Navigation.PushAsync(new SavingGoalsPage());
                    break;
                case "💸 Debt Management":
                    await Navigation.PushAsync(new DebtManagementPage());
                    break;
            }

            ((CollectionView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Clear navigation stack and go to login page
        await Shell.Current.GoToAsync("LoginPage");
    }
}
