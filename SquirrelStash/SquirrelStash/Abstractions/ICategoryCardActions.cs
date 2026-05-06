using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Defines operations that a category card can request from its owning context.
/// </summary>
public interface ICategoryCardActions
{
    /// <summary>
    /// Edits the specified category.
    /// </summary>
    /// <param name="category">The category to edit.</param>
    Task EditCategoryAsync(Category category);
}
