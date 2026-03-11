using FluentResults;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Abstractions;

public interface IItemsService
{
    Task AddItemAsync(Item item);

    Task UpdateItemAsync(Item item);
    
    Task RemoveItemAsync(int id);
    
    Task<Result<int>> IncreaseQuantityAsync(int id, int increment = 1);
    

    Task<Result<int>> DecreaseQuantityAsync(int id, int decrement = 1);
}