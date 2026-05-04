using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories;

internal class ItemCardViewModelFactory(IItemsService itemService, ILogger<ItemCardViewModel> logger)
    : IItemCardViewModelFactory
{
    /// <inheritdoc />
    public ItemCardViewModel GetViewModel(Item item, IItemCardActions itemCardActions)
    {
        return new ItemCardViewModel(item, itemService, itemCardActions, logger);
    }
}
