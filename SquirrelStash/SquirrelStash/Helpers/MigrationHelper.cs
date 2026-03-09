using Microsoft.EntityFrameworkCore;
using SquirrelStash.DataAccess;

namespace SquirrelStash.Helpers
{
    internal static class MigrationHelper
    {
        public static MauiApp EnsureMigrations(this MauiApp app)
        {
            using var scope = app.Services.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<StashContext>>();
            using var db = factory.CreateDbContext();

            db.Database.MigrateAsync().GetAwaiter().GetResult();

            return app;
        }
    }
}
