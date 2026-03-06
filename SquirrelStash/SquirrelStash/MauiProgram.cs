using Microsoft.Extensions.Logging;
using SquirrelStash.DataAccess.Helpers;


namespace SquirrelStash
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.ConfigureDbContext(Path.Combine(FileSystem.AppDataDirectory, "squirrelstash.db"));

            return builder.Build();
        }
    }
}
