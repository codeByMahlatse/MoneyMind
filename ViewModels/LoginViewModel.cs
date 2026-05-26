using FinalApp;
using FinalApp.Helpers;
using FinalApp.Models;
using FinalApp.Services;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace FinalApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DatabaseService _database;
        private string _email;
        private string _password;
        private bool _isBusy;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel(DatabaseService database)
        {
            _database = database;

            LoginCommand = new Command(async () => await LoginAsync(),
                () => !IsBusy);

            GoToRegisterCommand = new Command(async () => await GoToRegister());

            this.PropertyChanged += (_, __) => ((Command)LoginCommand).ChangeCanExecute();
        }

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter both email and password", "OK");
                    return;
                }

                var user = await _database.GetUserByEmail(Email);

                if (user == null || user.Password != PasswordHelper.HashPassword(Password))
                // In production, compare hashes
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid credentials", "OK");
                    return;
                }

                // Store user session if needed
                //App.CurrentUser = user;

                // Navigate to main page
                await Shell.Current.GoToAsync("//HomePage");

                // Clear sensitive data
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        /*private async void onSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("HomePage");
        }

        private async void onSubmitClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("HomePage");
        }*/
        public string username { get; set; }
        public string password { get; set; }
        public async Task<bool> ValidateLoginAsync()
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password", "OK");
            return false;
        }
       
    }
}

