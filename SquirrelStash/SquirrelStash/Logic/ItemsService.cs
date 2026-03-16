using FluentResults;
using Microsoft.EntityFrameworkCore;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Logic
{
    internal class ItemsService(IGenericDataSource<Item> dataSource) : IItemsService
    {
        public async Task<Result<IReadOnlyList<Item>>> GetCategoryItemsAsync(int categoryId)
        {
            try
            {
                var data = await dataSource.GetQueryableItems().Where(x => x.CategoryId == categoryId)
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

                var resultItem = await dataSource.AddAsync(newItem);

                return Result.Ok(resultItem);
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
            //TODO: Temp
            return Result.Ok(10);
        }

        public async Task<Result<int>> DecreaseQuantityAsync(int id, int decrement = 1)
        {
            return Result.Ok(10);
        }
    }
}
