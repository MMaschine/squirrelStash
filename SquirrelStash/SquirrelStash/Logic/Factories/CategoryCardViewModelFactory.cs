using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;

namespace SquirrelStash.Logic.Factories
{
    internal class CategoryCardViewModelFactory(
        IItemsService itemService,
        IItemCardViewModelFactory itemCardViewModelFactory,
        ICreateItemDialogFactory createItemDialogFactory,
        ILogger<CategoryCardViewModel> logger)
        : ICategoryCardViewModelFactory
    {
        /// <inheritdoc />
        public CategoryCardViewModel GetViewModel(Category category)
        {
            return new CategoryCardViewModel(
                category,
                itemService,
                itemCardViewModelFactory,
                createItemDialogFactory,
                logger);
        }
    }
}
