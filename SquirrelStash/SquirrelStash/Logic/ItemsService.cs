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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<Result<Item>> AddItemAsync(EditItemRequest editItemRequest)
        {
            try
            {
                var newItem = new Item()
                {
                    CategoryId = editItemRequest.CategoryId,
                    CriticalThreshold = editItemRequest.CriticalThreshold,
                    WarningThreshold = editItemRequest.WarningThreshold,
                    ImageSource = editItemRequest.ImageSource,
                    Quantity = editItemRequest.DefaultQuantity,
                    Note = editItemRequest.Note
                };

                newItem.PropertyEntries.AddRange(editItemRequest.Entries.Select(x => new PropertyEntry()
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
                await MessageHelper.NotifyException(e, $"Failed to add item to category {editItemRequest.CategoryId}", logger);
                return Result.Fail(AppText.FailedToAddItem);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Item>> UpdateItemAsync(EditItemRequest request)
        {
            if (!request.IsEdit)
            {
                throw new InvalidOperationException("Not an edit request");
            }

            try
            {
                var item = await _itemSet.Include(x=>x.PropertyEntries).FirstOrDefaultAsync(x=> x.Id == request.ItemId);

                if (item == null)
                {
                    return Result.Fail($"There is no item with id {request.ItemId} to be edited");
                }

                item.CriticalThreshold = request.CriticalThreshold;
                item.WarningThreshold = request.WarningThreshold;
                item.ImageSource = request.ImageSource;
                item.Quantity = request.DefaultQuantity;
                item.Note = request.Note;

                foreach (var requestEntry in request.Entries)
                {
                    var existingEntry = item.PropertyEntries.FirstOrDefault(x => x.PropertyDefinitionId == requestEntry.PropertyDefinitionId);

                    if (existingEntry == null)
                    {
                        item.PropertyEntries.Add(new PropertyEntry()
                        {
                            PropertyDefinitionId = requestEntry.PropertyDefinitionId,
                            Value = requestEntry.Value
                        });
                    }
                    else
                    {
                        existingEntry.Value = requestEntry.Value;
                    }
                }

                await context.SaveChangesAsync();
                return Result.Ok(item);
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to update item from category {request.CategoryId} with Id: {request.ItemId}", logger);
                return Result.Fail(AppText.FailedToUpdateItem);
            }
        }

        /// <inheritdoc />
        public async Task<Result> RemoveItemAsync(int id)
        {
            try
            {
                var item = await _itemSet.FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                {
                    logger.LogWarning("Failed to remove item because item {ItemId} was not found.", id);
                    return Result.Fail(AppText.ItemNotFound);
                }

                _itemSet.Remove(item);
                await context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception e)
            {
                await MessageHelper.NotifyException(e, $"Failed to remove item {id}.", logger);
                return Result.Fail(AppText.FailedToDeleteItem);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
