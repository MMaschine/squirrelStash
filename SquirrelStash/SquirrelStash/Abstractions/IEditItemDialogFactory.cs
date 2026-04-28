using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Views;

namespace SquirrelStash.Abstractions;

public interface IEditItemDialogFactory
{
    /// <summary>
    /// Creates an item creation dialog for the provided category.
    /// </summary>
    /// <param name="category">The category that will own the created item.</param>
    /// <returns>A configured item creation dialog.</returns>
    EditItemDialog CreateDialog(Category category);

    /// <summary>
    /// Creates an item editing dialog for the provided category and item.
    /// </summary>
    /// <param name="category">The category that owns the item.</param>
    /// <param name="item">The item to edit.</param>
    /// <returns>A configured item editing dialog.</returns>
    EditItemDialog CreateDialog(Category category, Item item);
}
