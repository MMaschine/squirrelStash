using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Logic;

namespace SquirrelStash.Tests;

public sealed class ItemsServiceTests
{
    [Test]
    public async Task DecreaseQuantityAsync_WhenDecrementIsNotPositive_FailsAndDoesNotChangeQuantity()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<StashContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new StashContext(options);
        await context.Database.EnsureCreatedAsync();

        var category = new Category
        {
            Title = "Parts",
            Items =
            [
                new Item
                {
                    Quantity = 5,
                    WarningThreshold = 2,
                    CriticalThreshold = 1
                }
            ]
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var itemId = category.Items.Single().Id;
        var service = new ItemsService(context, NullLogger<ItemsService>.Instance);

        var result = await service.DecreaseQuantityAsync(itemId, 0);

        Assert.That(result.IsFailed, Is.True);
        Assert.That(await context.Items.Where(x => x.Id == itemId).Select(x => x.Quantity).SingleAsync(), Is.EqualTo(5));
    }
}
