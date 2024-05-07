using Microsoft.Extensions.Logging;

namespace Homework3
{
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

    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();
            builder.Services.AddTransient<CustomerManagementPage>();
            builder.Services.AddTransient<MainPage>();


            return builder.Build();
        }
    }
}
