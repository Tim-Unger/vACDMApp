using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;
using CommunityToolkit.Maui;
using Plugin.LocalNotification;

namespace VacdmApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBottomSheet()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(
                    fonts =>
                    {
                        fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                        fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                        fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                        fonts.AddFont("AdvancedDot-Regular.ttf", "AdvancedDot");
                    }
                );

            return builder.Build();
        }
    }
}
