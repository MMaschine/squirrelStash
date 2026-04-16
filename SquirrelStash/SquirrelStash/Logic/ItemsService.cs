using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Requests;
using SquirrelStash.Resources;

namespace SquirrelStash.Logic
{
    internal class ItemsService(StashContext context, ILogger<ItemsService> logger) : IItemsService
    {
        private readonly DbSet<Item> _itemSet = context.Set<Item>();

        public async Task<Result<IReadOnlyList<Item>>> GetCategoryItemsAsync(int categoryId)
        {
            try
            {
                var data = await _itemSet.AsQueryable().Where(x => x.CategoryId == categoryId)
                    .Include(x => x.PropertyEntries).ToListAsync() ?? [];

                return data.AsReadOnly();
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to load items for category {categoryId}.", logger);
                return Result.Fail(AppText.CannotGetCategories);
            }
        }

        public async Task<Result<Item>> AddItemAsync(int categoryId, CreateItemRequest createItemRequest)
        {
            try
            {
                var newItem = new Item()
                {
                    CategoryId = categoryId,
                    CriticalThreshold = createItemRequest.CriticalThreshold,
                    WarningThreshold = createItemRequest.WarningThreshold,
                    ImageSource = createItemRequest.ImageSource,
                    Quantity = createItemRequest.DefaultQuantity,
                    Note = createItemRequest.Note
                };

                newItem.PropertyEntries.AddRange(createItemRequest.Entries.Select(x => new PropertyEntry()
                {
                    PropertyDefinitionId = x.PropertyDefinitionId,
                    Value = x.Value
                }));

                var resultItem = await _itemSet.AddAsync(newItem);
                await context.SaveChangesAsync();

                return Result.Ok(resultItem.Entity);
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to add item to category {categoryId}", logger);
                return Result.Fail(AppText.FailedToCreateItem);
            }
        }

        public async Task UpdateItemAsync(Item item)
        {
        }

        public async Task RemoveItemAsync(int id)
        {
        }

        public async Task<Result<int>> IncreaseQuantityAsync(int id, int increment = 1)
        {
            if (increment <= 0)
            {
                logger.LogWarning("Rejected quantity increase for item {ItemId} because increment {Increment} is invalid.", id, increment);
                return Result.Fail(AppText.WrongIncrement);
            }

            try
            {
                var item = await _itemSet.FindAsync(id);

                if (item != null)
                {
                    item.Quantity += increment;
                    await context.SaveChangesAsync();
                    return Result.Ok(item.Quantity);
                }
                else
                {
                    logger.LogWarning("Failed to increase quantity because item {ItemId} was not found.", id);
                    return Result.Fail(AppText.ItemNotFound);
                }
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to increase quantity for item {id} by {increment}.", logger);
                return Result.Fail(AppText.FailedToUpdateItem);
            }
        }

        public async Task<Result<int>> DecreaseQuantityAsync(int id, int decrement = 1)
        {
            try
            {
                var item = await _itemSet.FindAsync(id);

                if (item != null)
                {
                    var newQuantity = item.Quantity - decrement;
                    item.Quantity = newQuantity >= 0 ? newQuantity : 0;
                    await context.SaveChangesAsync();

                    return Result.Ok(item.Quantity);
                }
                else
                {
                    logger.LogWarning("Failed to decrease quantity because item {ItemId} was not found.", id);
                    return Result.Fail(AppText.ItemNotFound);
                }
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to decrease quantity for item {id} by {decrement}.", logger);
                return Result.Fail(AppText.FailedToUpdateItem);
            }
        }
    }
}
