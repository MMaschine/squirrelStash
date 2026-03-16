using FluentResults;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Abstractions;

public interface IItemsService
{
    Task<Result<IReadOnlyList<Item>>> GetCategoryItemsAsync(int categoryId);

    Task<Result<Item>> AddItemAsync(int categoryId, CreateItemRequest createItemRequest);

    Task UpdateItemAsync(Item item);
    
    Task RemoveItemAsync(int id);
    
    Task<Result<int>> IncreaseQuantityAsync(int id, int increment = 1);
    

    Task<Result<int>> DecreaseQuantityAsync(int id, int decrement = 1);
}