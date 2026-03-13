using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories
{
    public sealed class ItemCardViewModelFactory(
        IItemsService itemsService) : IItemCardViewModelFactory
    {
        private readonly IItemsService _itemsService = itemsService;

        public ItemCardViewModel Create(Item item)
        {
            return new ItemCardViewModel(item, _itemsService);
        }
    }
}
