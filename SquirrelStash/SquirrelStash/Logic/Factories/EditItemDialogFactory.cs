using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;
using SquirrelStash.Views;

namespace SquirrelStash.Logic.Factories;

internal class EditItemDialogFactory(IImageService imageService)
    : IEditItemDialogFactory
{
    /// <inheritdoc />
    public EditItemDialog CreateDialog(Category category)
    {
        var viewModel = new EditItemDialogViewModel(category, imageService);
        return new EditItemDialog(viewModel);
    }

    /// <inheritdoc />
    public EditItemDialog CreateDialog(Category category, Item item)
    {
        var viewModel = new EditItemDialogViewModel(category, item, imageService);
        return new EditItemDialog(viewModel);
    }
}
