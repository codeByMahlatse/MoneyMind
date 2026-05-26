using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using FinalApp.Models;
using FinalApp.Services;

namespace FinalApp
{
    public partial class SavingGoalsPage : ContentPage
    {
        public ObservableCollection<SavingGoal> SavingGoals { get; set; }
        private readonly DatabaseService _databaseService = new DatabaseService();

        public SavingGoalsPage()
        {
            InitializeComponent();

            SavingGoals = new ObservableCollection<SavingGoal>
            {
                new SavingGoal { Name = "Holiday", CurrentAmount = 3000, TargetAmount = 12000 },
                new SavingGoal { Name = "New Bike", CurrentAmount = 6000, TargetAmount = 15500 },
                new SavingGoal { Name = "Emergency Fund", CurrentAmount = 1500, TargetAmount = 4500 }
            };

            BindingContext = this;
        }

        private async void OnAddGoalClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Goal", "Enter goal name:");
            if (string.IsNullOrWhiteSpace(name))
                return;

            string initialInput = await DisplayPromptAsync("Initial Saved", "Enter saved amount:", keyboard: Keyboard.Numeric);
            if (!decimal.TryParse(initialInput, out decimal savedAmount) || savedAmount < 0)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid saved amount.", "OK");
                return;
            }

            string targetInput = await DisplayPromptAsync("Target Amount", "Enter target amount:", keyboard: Keyboard.Numeric);
            if (!decimal.TryParse(targetInput, out decimal targetAmount) || targetAmount <= 0)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid target amount greater than zero.", "OK");
                return;
            }

            if (savedAmount > targetAmount)
            {
                await DisplayAlert("Warning", "Saved amount cannot exceed target amount.", "OK");
                return;
            }

            var newGoal = new SavingGoal
            {
                Name = name,
                CurrentAmount = savedAmount,
                TargetAmount = targetAmount
            };

            SavingGoals.Add(newGoal); // update UI

            bool success = await _databaseService.AddSavingGoalAsync(newGoal);
            if (!success)
            {
                await DisplayAlert("Database Error", "Failed to save goal to database.", "OK");
            }
        }
    }
}
