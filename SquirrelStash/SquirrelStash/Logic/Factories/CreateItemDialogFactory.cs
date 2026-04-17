using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;
using SquirrelStash.Views;

namespace SquirrelStash.Logic.Factories;

internal class CreateItemDialogFactory(IImageService imageService)
    : ICreateItemDialogFactory
{
    /// <inheritdoc />
    public CreateItemDialog CreateDialog(Category category)
    {
        var viewModel = new CreateItemDialogViewModel(category, imageService);
        return new CreateItemDialog(viewModel);
    }
}
