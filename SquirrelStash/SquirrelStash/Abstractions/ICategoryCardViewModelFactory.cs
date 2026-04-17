using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Abstractions;

public interface ICategoryCardViewModelFactory
{
    /// <summary>
    /// Builds a view model for the provided category.
    /// </summary>
    /// <param name="category">The category entity used to initialize the view model.</param>
    /// <returns>A category card view model.</returns>
    CategoryCardViewModel GetViewModel(Category category);
}
