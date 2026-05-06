using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;

namespace SquirrelStash.Logic.Factories
{
    internal class CategoryCardViewModelFactory(
        IItemsService itemService,
        IItemCardViewModelFactory itemCardViewModelFactory,
        IEditItemDialogFactory editItemDialogFactory,
        IModalDialogService modalDialogService,
        ILogger<CategoryCardViewModel> logger)
        : ICategoryCardViewModelFactory
    {
        /// <inheritdoc />
        public CategoryCardViewModel GetViewModel(Category category, ICategoryCardActions categoryCardActions)
        {
            return new CategoryCardViewModel(
                category,
                itemService,
                itemCardViewModelFactory,
                editItemDialogFactory,
                modalDialogService,
                categoryCardActions,
                logger);
        }
    }
}
