using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using STFREYA.Services;

namespace STFREYA
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                    fonts.AddFont("Lexend-Black.ttf", "LexendBlack");
                    fonts.AddFont("Lexend-Bold.ttf", "LexendBold");
                    fonts.AddFont("Lexend-ExtraBold.ttf", "LexendExtraBold");
                    fonts.AddFont("Lexend-ExtraLight.ttf", "LexendExtraLight");
                    fonts.AddFont("Lexend-Light.ttf", "LexendLight");
                    fonts.AddFont("Lexend-Medium.ttf", "LexendMedium");
                    fonts.AddFont("Lexend-Regular.ttf", "LexendRegular");
                    fonts.AddFont("Lexend-SemiBold.ttf", "LexendSemiBold");
                    fonts.AddFont("Lexend-Thin.ttf", "LexendThin");
                });


#if DEBUG
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}

