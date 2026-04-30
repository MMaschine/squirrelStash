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
    /// <param name="itemCardActions">The owner actions available to the item card.</param>
    /// <returns>An item card view model.</returns>
    ItemCardViewModel GetViewModel(Item item, IItemCardActions itemCardActions);
}
