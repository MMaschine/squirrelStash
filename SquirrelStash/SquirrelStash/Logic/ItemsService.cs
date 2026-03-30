using FluentResults;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Logic
{
    internal class ItemsService(StashContext context) : IItemsService
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
                //TODO: add log
                return Result.Fail("Can't get categories");
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
                    Note = createItemRequest.Note
                };

                newItem.PropertyEntries.AddRange(createItemRequest.Entries.Select(x => new PropertyEntry()
                {
                    PropertyDefinitionId = x.PropertyDefinitionId,
                    Value = x.Value
                }));

                var resultItem = await _itemSet.AddAsync(newItem);

                return Result.Ok(resultItem.Entity);
            }
            catch (Exception e)
            {
                //TODO: add logging
                return Result.Fail("Failed to create item");
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
                return Result.Fail("Wrong increment");
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
                    //TODO: add log
                    return Result.Fail("Item not found");
                }
            }
            catch (Exception e)
            {
                //TODO: add log
                return Result.Fail("Failed to update item");
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
                    //TODO: add log
                    return Result.Fail("Item not found");
                }
            }
            catch (Exception e)
            {
                //TODO: add log
                return Result.Fail("Failed to update item");
            }
        }

    }
}
