using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SquirrelStash.DataAccess.Infrastructure;

/// <summary>
/// Infrastructure to init DB context in design time
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StashContext>
{
    public StashContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StashContext>();
        optionsBuilder.UseSqlite("Data Source=squirrelstash-dev.db");

        return new StashContext(optionsBuilder.Options);
    }
}