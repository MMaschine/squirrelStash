using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;
using SquirrelStash.Views;

namespace SquirrelStash.Logic.Factories;

internal class EditCategoryDialogFactory : IEditCategoryDialogFactory
{
    /// <inheritdoc />
    public EditCategoryDialog GetDialogToCreate(string[] existingTitles)
    {
        var viewModel = new EditCategoryDialogViewModel(existingTitles);
        return new EditCategoryDialog(viewModel);
    }

    /// <inheritdoc />
    public EditCategoryDialog GetDialogToEdit(string[] existingTitles, Category category)
    {
        var viewModel = new EditCategoryDialogViewModel(existingTitles, category);
        return new EditCategoryDialog(viewModel);
    }
}
