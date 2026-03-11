using FluentResults;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Logic
{
    internal class ItemsService : IItemsService
    {
        public async Task AddItemAsync(Item item)
        {
            
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
