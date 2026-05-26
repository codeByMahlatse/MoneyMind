using System;
using Microsoft.Maui.Controls;
using FinalApp.Models;
using FinalApp.Services;
using System.Threading.Tasks;

namespace FinalApp
{
    public partial class DebtManagementPage : ContentPage
    {
        private decimal totalOwed = 0;
        private readonly DatabaseService _databaseService = new DatabaseService();

        public DebtManagementPage()
        {
            InitializeComponent();
            LoadDebtsFromDatabase();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && !string.IsNullOrWhiteSpace(ToWhomEntry.Text))
            {
                var debt = new Debt
                {
                    Creditor = ToWhomEntry.Text,
                    Amount = amount
                };

                bool success = await _databaseService.AddDebtAsync(debt);

                if (success)
                {
                    AddDebtItem(debt);
                    totalOwed += debt.Amount;
                    UpdateTotalOwedLabel();

                    AmountEntry.Text = string.Empty;
                    ToWhomEntry.Text = string.Empty;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save debt to database.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid amount and name.", "OK");
            }
        }

        private void AddDebtItem(Debt debt)
        {
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 5)
            };

            var nameLabel = new Label
            {
                Text = debt.Creditor,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Black
            };

            var amountLabel = new Label
            {
                Text = $"R{debt.Amount}",
                VerticalOptions = LayoutOptions.Center
            };

            var spacer = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var markPaidButton = new Button
            {
                Text = "Mark as Paid",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Blue,
                FontAttributes = FontAttributes.Bold
            };

            markPaidButton.Clicked += async (s, e) =>
            {
                bool deleted = await _databaseService.DeleteDebtAsync(debt.Id);
                if (deleted)
                {
                    DebtList.Children.Remove(stack);
                    totalOwed -= debt.Amount;
                    UpdateTotalOwedLabel();
                }
                else
                {
                    await DisplayAlert("Error", "Could not delete the debt from database.", "OK");
                }
            };

            stack.Children.Add(nameLabel);
            stack.Children.Add(amountLabel);
            stack.Children.Add(spacer);
            stack.Children.Add(markPaidButton);

            DebtList.Children.Add(stack);
        }

        private void UpdateTotalOwedLabel()
        {
            TotalOwedLabel.Text = $"R{totalOwed:N0}";
        }

        private async void LoadDebtsFromDatabase()
        {
            var debts = await _databaseService.GetDebtsAsync();
            foreach (var debt in debts)
            {
                AddDebtItem(debt);
                totalOwed += debt.Amount;
            }
            UpdateTotalOwedLabel();
        }
    }
}
