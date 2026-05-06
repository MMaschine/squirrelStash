using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Logic;

internal sealed class ItemCardActionsAdapter(
    Func<Item, Task> editItemAsync,
    Func<Item, Task> deleteItemAsync,
    Func<Item, Task> copyItemAsync)
    : IItemCardActions
{
    /// <inheritdoc />
    public Task EditItemAsync(Item item)
    {
        return editItemAsync(item);
    }

    /// <inheritdoc />
    public Task DeleteItemAsync(Item item)
    {
        return deleteItemAsync(item);
    }

    /// <inheritdoc />
    public Task CopyItemAsync(Item item)
    {
        return copyItemAsync(item);
    }
}
