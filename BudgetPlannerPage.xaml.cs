using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using FinalApp.Models;
using FinalApp.Services;

namespace FinalApp
{
    public partial class BudgetPlannerPage : ContentPage
    {
        public BudgetPlannerPage()
        {
            InitializeComponent();
        }

        private async void OnGenerateChartClicked(object sender, EventArgs e)
        {
            float income = ParseEntry(IncomeEntry);
            float rent = ParseEntry(RentEntry);
            float grocery = ParseEntry(GroceryEntry);
            float transport = ParseEntry(TransportEntry);
            float utilities = ParseEntry(UtilitiesEntry);

            float totalExpenses = rent + grocery + transport + utilities;
            float savings = income - totalExpenses;
            float savingsPercentage = income > 0 ? (savings / income) * 100 : 0;

            // Strategy Suggestion
            if (income == 0)
            {
                StrategyLabel.Text = "Please enter a valid income.";
                StrategyLabel.TextColor = Colors.Red;
            }
            else if (savings < 0)
            {
                StrategyLabel.Text = $" Your expenses exceed your income by R{-savings:F2}. Consider reducing non-essential costs.";
                StrategyLabel.TextColor = Colors.Red;
            }
            else if (savingsPercentage < 10)
            {
                StrategyLabel.Text = $"You are saving {savingsPercentage:F1}%. Try to aim for 20%+ savings by adjusting expenses.";
                StrategyLabel.TextColor = Colors.Orange;
            }
            else
            {
                StrategyLabel.Text = $" Good job! You're saving {savingsPercentage:F1}% of your income.";
                StrategyLabel.TextColor = Colors.Green;
            }

            // Pie chart entries
            var entries = new List<ChartEntry>
            {
                new ChartEntry(rent) { Label = "Rent", ValueLabel = rent.ToString("F2"), Color = SKColor.Parse("#266489") },
                new ChartEntry(grocery) { Label = "Grocery", ValueLabel = grocery.ToString("F2"), Color = SKColor.Parse("#68B9C0") },
                new ChartEntry(transport) { Label = "Transport", ValueLabel = transport.ToString("F2"), Color = SKColor.Parse("#90D585") },
                new ChartEntry(utilities) { Label = "Utilities", ValueLabel = utilities.ToString("F2"), Color = SKColor.Parse("#F3C151") },
                new ChartEntry(savings) { Label = "Savings", ValueLabel = savings.ToString("F2"), Color = SKColor.Parse("#A65FDD") }
            };

            PieChart.Chart = new PieChart
            {
                Entries = entries,
                LabelTextSize = 35,
                BackgroundColor = SKColors.White
            };

            var budget = new BudgetModel
            {
                Income = income,
                Rent = rent,
                Grocery = grocery,
                Transport = transport,
                Utilities = utilities,
                Savings = savings,
                CreatedAt = DateTime.Now
            };

            var saved = await App.DatabaseService.SaveBudgetAsync(budget);

            if (saved)
            {
                await DisplayAlert("Success", "Your budget has been saved.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to save your budget.", "OK");
            }

        }

        private float ParseEntry(Entry entry)
        {
            return float.TryParse(entry.Text, out var value) ? value : 0f;
        }
    }
}
