using System.ComponentModel;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;

namespace FinalApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;

        public DashboardViewModel(string userEmail)
        {
            _dbService = new DatabaseService();
            _ = LoadBudgetDataAsync(userEmail);
        }

        private async Task LoadBudgetDataAsync(string email)
        {
            var budgets = await _dbService.GetBudgetsForUserAsync(email);
            var latestBudget = budgets?.Count > 0 ? budgets[0] : null;

            if (latestBudget != null)
            {
                Income = latestBudget.Income;
                TotalExpenses = latestBudget.Rent + latestBudget.Grocery + latestBudget.Transport + latestBudget.Utilities;
                TotalDebt = latestBudget.Debt;
                TotalSavings = latestBudget.Savings;
            }
            else
            {
                ResetFinancialData();
            }
        }

        public void ResetFinancialData()
        {
            Income = 0;
            TotalExpenses = 0;
            TotalDebt = 0;
            TotalSavings = 0;
        }

        private double income;
        public double Income
        {
            get => income;
            set
            {
                if (income != value)
                {
                    income = value;
                    OnPropertyChanged(nameof(Income));
                }
            }
        }

        private double totalExpenses;
        public double TotalExpenses
        {
            get => totalExpenses;
            set
            {
                if (totalExpenses != value)
                {
                    totalExpenses = value;
                    OnPropertyChanged(nameof(TotalExpenses));
                }
            }
        }

        private double totalDebt;
        public double TotalDebt
        {
            get => totalDebt;
            set
            {
                if (totalDebt != value)
                {
                    totalDebt = value;
                    OnPropertyChanged(nameof(TotalDebt));
                }
            }
        }

        private double totalSavings;
        public double TotalSavings
        {
            get => totalSavings;
            set
            {
                if (totalSavings != value)
                {
                    totalSavings = value;
                    OnPropertyChanged(nameof(TotalSavings));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
