using FinalApp.Models;
using FinalApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace FinalApp
{
    public partial class FinancialReportPage : ContentPage
    {
        private readonly DatabaseService _dbService = new DatabaseService();

        public class ReportViewModel
        {
            public string UserName { get; set; } = "Financial Report";
            public string ReportDate { get; set; } = DateTime.Now.ToString("MMMM dd, yyyy");
            public string Reportgetting => "Financial Report";

            // Budget
            public double Income { get; set; }
            public double TotalExpenses { get; set; }
            public double RemainingBudget => Income - TotalExpenses;

            // Savings
            public decimal EmergencyFund { get; set; }
            public decimal RetirementFund { get; set; }
            public decimal OtherSavings { get; set; }
            public decimal TotalSavings => EmergencyFund + RetirementFund + OtherSavings;

            // Debt
            public List<Debt> Debts { get; set; } = new();
            public decimal TotalDebt => Debts?.Sum(d => d.Amount) ?? 0;
        }

        public FinancialReportPage()
        {
            InitializeComponent();
            LoadReport();
        }

        private async void LoadReport()
        {
            var viewModel = new ReportViewModel();

            // Load User
            var user = await _dbService.GetUserByEmail("johndoe@email.com"); // replace dynamically as needed
            if (user != null)
                viewModel.UserName = user.Name;

            // Load Budget
            var budgets = await _dbService.GetAllBudgetsAsync();
            var latestBudget = budgets.FirstOrDefault();
            if (latestBudget != null)
            {
                viewModel.Income = latestBudget.Income;
                viewModel.TotalExpenses = latestBudget.Rent + latestBudget.Grocery + latestBudget.Transport + latestBudget.Utilities;
            }

            // Load Savings
            var savings = await _dbService.GetSavingGoalsAsync();
            viewModel.EmergencyFund = savings.FirstOrDefault(s => s.Name.Contains("Emergency", StringComparison.OrdinalIgnoreCase))?.CurrentAmount ?? 0;
            viewModel.RetirementFund = savings.FirstOrDefault(s => s.Name.Contains("Retirement", StringComparison.OrdinalIgnoreCase))?.CurrentAmount ?? 0;
            viewModel.OtherSavings = savings
                .Where(s => !s.Name.Contains("Emergency", StringComparison.OrdinalIgnoreCase) &&
                            !s.Name.Contains("Retirement", StringComparison.OrdinalIgnoreCase))
                .Sum(s => s.CurrentAmount);

            // Load Debts
            viewModel.Debts = await _dbService.GetDebtsAsync();

            BindingContext = viewModel;
        }
    }
}
