using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Creates item card view models from item entities.
/// </summary>
public interface IItemCardViewModelFactory
{
    /// <summary>
    /// Builds a view model for the provided item.
    /// </summary>
    /// <param name="item">The item entity used to initialize the view model.</param>
    /// <param name="editAction">The action invoked when the item should be edited.</param>
    /// <returns>An item card view model.</returns>
    ItemCardViewModel GetViewModel(Item item, Func<Item, Task> editAction);
}
