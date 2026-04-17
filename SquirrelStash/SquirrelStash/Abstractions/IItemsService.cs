using FluentResults;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Abstraction of the service providing item data operations.
/// </summary>
public interface IItemsService
{
    /// <summary>
    /// Gets all items for a category.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <returns>A result containing the category items.</returns>
    Task<Result<IReadOnlyList<Item>>> GetCategoryItemsAsync(int categoryId);

    /// <summary>
    /// Adds a new item to a category.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <param name="createItemRequest">The item creation request.</param>
    /// <returns>A result containing the created item.</returns>
    Task<Result<Item>> AddItemAsync(int categoryId, CreateItemRequest createItemRequest);

    /// <summary>
    /// Updates an existing item.
    /// </summary>
    /// <param name="item">The item to update.</param>
    Task UpdateItemAsync(Item item);

    /// <summary>
    /// Removes an item by identifier.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    Task RemoveItemAsync(int id);

    /// <summary>
    /// Increases an item's quantity.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <param name="increment">The amount to add to the quantity.</param>
    /// <returns>A result containing the updated quantity.</returns>
    Task<Result<int>> IncreaseQuantityAsync(int id, int increment = 1);

    /// <summary>
    /// Decreases an item's quantity.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <param name="decrement">The amount to subtract from the quantity.</param>
    /// <returns>A result containing the updated quantity.</returns>
    Task<Result<int>> DecreaseQuantityAsync(int id, int decrement = 1);
}
