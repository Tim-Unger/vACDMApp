using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;
using CommunityToolkit.Maui;

namespace VACDMApp
{
    public static class MauiProgram
    {
        public static HttpClient Client;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBottomSheet()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("AdvancedDot-Regular.ttf", "AdvancedDot");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}