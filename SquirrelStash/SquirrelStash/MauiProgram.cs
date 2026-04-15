using Microsoft.Extensions.Logging;
using Serilog;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Helpers;
using SquirrelStash.Helpers;
using SquirrelStash.Logic;
using SquirrelStash.Logic.Factories;
using SquirrelStash.ViewModels;
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
            //Serilog configuration 
            builder.Services.AddSerilog(new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger());

            builder.Services.ConfigureDbContext(Path.Combine(FileSystem.AppDataDirectory, "squirrelstash.db"));

            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IItemsService, ItemsService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOverviewService, OverviewService>();
            builder.Services.AddScoped<ICreateItemDialogFactory, CreateItemDialogFactory>();
            builder.Services.AddScoped<IItemCardViewModelFactory, ItemCardViewModelFactory>();
            builder.Services.AddScoped<ICategoryCardViewModelFactory, CategoryCardViewModelFactory>();
            builder.Services.AddScoped<IOverviewThresholdItemViewModelFactory, OverviewThresholdItemViewModelFactory>();

            builder.Services.AddViewWithViewModel<OverviewPage, OverviewPageViewModel>();
            builder.Services.AddViewWithViewModel<TreePage, TreePageViewModel>();
            

            return builder.Build().EnsureMigrations();
        }
    }
}
