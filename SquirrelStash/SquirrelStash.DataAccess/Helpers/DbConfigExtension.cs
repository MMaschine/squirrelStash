using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace SquirrelStash.DataAccess.Helpers
{
    public static class DbConfigurationExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, string dbPath)
        {
            services.AddDbContextFactory<StashContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

        }
    }
}
