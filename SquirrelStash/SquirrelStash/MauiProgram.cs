using Microsoft.Extensions.Logging;
using SquirrelStash.DataAccess.Helpers;
using SquirrelStash.Helpers;
using SquirrelStash.ViewModel;
using SquirrelStash.Views;


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
            builder.Services.AddViewWithViewModel<TreePage, TreePageViewModel>();
            
            return builder.Build().EnsureMigrations();
        }
    }
}
