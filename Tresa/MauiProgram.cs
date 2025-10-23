using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Tresa.Services;
using Tresa.ViewModels;
using Tresa.Views;

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

            builder.Services.AddTresaServices()
                .AddTresaViewModels()
                .AddTresaPages();
          
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
