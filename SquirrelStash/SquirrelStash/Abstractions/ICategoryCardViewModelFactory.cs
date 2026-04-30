using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Abstractions;

/// <summary>
/// Creates category card view models from category entities.
/// </summary>
public interface ICategoryCardViewModelFactory
{
    /// <summary>
    /// Builds a view model for the provided category.
    /// </summary>
    /// <param name="category">The category entity used to initialize the view model.</param>
    /// <param name="categoryCardActions">The owner actions available to the category card.</param>
    /// <returns>A category card view model.</returns>
    CategoryCardViewModel GetViewModel(Category category, ICategoryCardActions categoryCardActions);
}
