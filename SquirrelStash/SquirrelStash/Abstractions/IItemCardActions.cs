using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Defines operations that an item card can request from its owning context.
/// </summary>
public interface IItemCardActions
{
    /// <summary>
    /// Edits the specified item.
    /// </summary>
    /// <param name="item">The item to edit.</param>
    Task EditItemAsync(Item item);

    /// <summary>
    /// Deletes the specified item.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    Task DeleteItemAsync(Item item);
}
