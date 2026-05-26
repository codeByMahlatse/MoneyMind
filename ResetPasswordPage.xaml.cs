using FinalApp.Services;
using Microsoft.Maui.Controls;
using System;

namespace FinalApp
{
    public partial class ResetPasswordPage : ContentPage
    {
        private DatabaseService _databaseService = new DatabaseService();

        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var user = await _databaseService.GetUserByEmail(email);
            if (user == null)
            {
                await DisplayAlert("Error", "Email not found.", "OK");
                return;
            }

            user.Password = newPassword;
            await _databaseService.UpdateUserAsync(user);// Since ID exists, this will update the user

            await DisplayAlert("Success", "Password has been reset.", "OK");
            await Navigation.PopAsync(); // Go back to login page or previous screen
        }
    }
}
