using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Tresa.Services;
using Tresa.Services.Implementations;
using Tresa.Services.Interfaces;

namespace Tresa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera() // <-- enables CameraView
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Services
            builder.Services.AddTransient<IStorageService, StorageService>();
            builder.Services.AddTransient<ICameraService, CameraService>();


            // ViewModels
            builder.Services.AddTransient<ViewModels.MainViewModel>();
            builder.Services.AddTransient<ViewModels.SettingsViewModel>();

            // Pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<Views.Pages.MainPage>();
            builder.Services.AddTransient<Views.Pages.SettingsPage>();
            builder.Services.AddTransient<Views.Pages.GalleryPage>();

            return builder.Build();
        }
    }
}
