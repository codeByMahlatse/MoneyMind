using FinalApp.Services;
using FinalApp.ViewModels;
using FinalApp.Helpers;
using System;
using System.Threading.Tasks;

namespace FinalApp;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        string email = UsernameEntery.Text?.Trim();
        string password = passwordEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Input Error", "Please enter both email and password.", "OK");
            return;
        }

        var user = await _databaseService.GetUserByEmail(email);
        if (user == null)
        {
            await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
            return;
        }

        // Compare hashed passwords
        string hashedInput = PasswordHelper.HashPassword(password);
        if (user.Password != hashedInput)
        {
            await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
            return;
        }

        // Clear sensitive data
        passwordEntry.Text = string.Empty;

        // Navigate to dashboard
        await Navigation.PushAsync(new dashboard(user.Email));
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Register());
    }

    private async void OnForgotClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordPage());
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        togglePasswordButton.Text = passwordEntry.IsPassword ? "Show" : "Hide";
    }

    
}
