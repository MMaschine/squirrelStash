using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Views;

namespace SquirrelStash.Abstractions;

public interface ICreateItemDialogFactory
{
    /// <summary>
    /// Creates an item creation dialog for the provided category.
    /// </summary>
    /// <param name="category">The category that will own the created item.</param>
    /// <returns>A configured item creation dialog.</returns>
    CreateItemDialog CreateDialog(Category category);
}
