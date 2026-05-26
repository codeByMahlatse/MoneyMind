using FinalApp.ViewModels;

namespace FinalApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            BindingContext = loginViewModel;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            // Use DI to get the LoginPage
            var loginPage = Handler.MauiContext.Services.GetService<LoginPage>();
            await Navigation.PushAsync(loginPage);
        }
    }
}