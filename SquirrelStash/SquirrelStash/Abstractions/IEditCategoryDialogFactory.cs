using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Views;

namespace SquirrelStash.Abstractions;

public interface IEditCategoryDialogFactory
{
    /// <summary>
    /// Creates a category dialog initialized for creating a new category.
    /// </summary>
    /// <param name="existingTitles">Existing category titles used for validation.</param>
    /// <returns>A configured category dialog.</returns>
    EditCategoryDialog GetDialogToCreate(string[] existingTitles);

    /// <summary>
    /// Creates a category dialog initialized for editing the provided category.
    /// </summary>
    /// <param name="existingTitles">Existing category titles used for validation.</param>
    /// <param name="category">The category used as initial edit data.</param>
    /// <returns>A configured category dialog.</returns>
    EditCategoryDialog GetDialogToEdit(string[] existingTitles, Category category);
}
