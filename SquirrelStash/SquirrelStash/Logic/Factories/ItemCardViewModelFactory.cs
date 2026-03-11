using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Logic.Factories
{
    public sealed class ItemCardViewModelFactory(
        IItemsService itemsService,
        IMessageService messageService) : IItemCardViewModelFactory
    {
        private readonly IItemsService _itemsService = itemsService;
        private readonly IMessageService _messageService = messageService;

        public ItemCardViewModel Create(Item item)
        {
            return new ItemCardViewModel(item, _itemsService, _messageService);
        }
    }
}
