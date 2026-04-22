using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Abstractions;

public interface ICategoryCardViewModelFactory
{
    /// <summary>
    /// Builds a view model for the provided category.
    /// </summary>
    /// <param name="category">The category entity used to initialize the view model.</param>
    /// <param name="editCategoryAction">Action to handle the press of the button "Edit Category"</param>
    /// <returns>A category card view model.</returns>
    CategoryCardViewModel GetViewModel(Category category, Func<Category, Task> editCategoryAction);
}
