using static System.Runtime.InteropServices.JavaScript.JSType;
using FinalApp.Services;
using FinalApp.Models;
using System.Security.Cryptography;
using System.Text;




namespace FinalApp;

public partial class Register : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();

    public Register()
	{
		InitializeComponent();

	}

    public static class PasswordHelper
    {
        // Hash the password with SHA256
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the password to a byte array and compute the hash
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a base64 string for storage
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Validate the input fields
        if (string.IsNullOrWhiteSpace(nameEntry.Text) ||
            string.IsNullOrWhiteSpace(emailEntry.Text) ||
            string.IsNullOrWhiteSpace(passwordEntry.Text) ||
            string.IsNullOrWhiteSpace(confirmPasswordEntry.Text) ||
            employmentPicker.SelectedIndex == -1 ||
            incomePicker.SelectedIndex == -1 ||
            goalsPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Missing Info", "Please fill out all required fields.", "OK");
            return;
        }

        // Name validation: only letters and spaces allowed
        if (!System.Text.RegularExpressions.Regex.IsMatch(nameEntry.Text, @"^[A-Za-z\s]+$"))
        {
            await DisplayAlert("Invalid Name", "Name should only contain letters and spaces.", "OK");
            return;
        }

        // Email validation using regex
        if (!System.Text.RegularExpressions.Regex.IsMatch(emailEntry.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
            return;
        }

        // Check if passwords match
        if (passwordEntry.Text != confirmPasswordEntry.Text)
        {
            await DisplayAlert("Password Mismatch", "Passwords do not match.", "OK");
            return;
        }

        // Check if user agreed to the terms
        if (!termsCheckBox.IsChecked)
        {
            await DisplayAlert("Agreement Required", "You must agree to the terms.", "OK");
            return;
        }

        // Hash the password before storing it
        string hashedPassword = PasswordHelper.HashPassword(passwordEntry.Text);

        await _databaseService.InitializeUserTable();

        var user = new User
        {
            Name = nameEntry.Text,
            Email = emailEntry.Text,
            Password = hashedPassword,
            EmploymentStatus = employmentPicker.SelectedItem.ToString(),
            IncomeLevel = incomePicker.SelectedItem.ToString(),
            FinancialGoal = goalsPicker.SelectedItem.ToString()
        };

        var success = await _databaseService.AddUser(user);

        if (!success)
        {
            await DisplayAlert("Error", "Registration failed. Try again.", "OK");
            return;
        }

        await DisplayAlert("Success", "You have registered successfully!", "OK");
        await Shell.Current.GoToAsync("LoginPage");
    }

    private bool isPasswordVisible = false;
    private bool isConfirmPasswordVisible = false;

   



}