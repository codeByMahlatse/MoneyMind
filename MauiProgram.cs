using FinalApp.ViewModels;
using FinalApp.Services;
using Microsoft.Extensions.Logging;
using Microcharts.Maui;

namespace FinalApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.UseMauiApp<App>()
       .UseMicrocharts(); // Add this if using Microcharts.Maui


#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register Services
        builder.Services.AddSingleton<DatabaseService>();

        // Register ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainPage>(); // If you have one

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}