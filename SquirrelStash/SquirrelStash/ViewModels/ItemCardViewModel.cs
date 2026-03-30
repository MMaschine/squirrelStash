using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;

namespace SquirrelStash.ViewModels
{
    public partial class ItemCardViewModel : ObservableObject
    {
        private readonly IItemsService _itemsService;


        private readonly int _itemId;

        public ItemCardViewModel(Item item, IItemsService itemService)
        {
            _itemsService = itemService;
            _itemId = item.Id;

            Quantity = item.Quantity;

            Name = string.Join(" ",
                item.PropertyEntries
                    .Select(p => p.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v)));

            ImagePath = string.IsNullOrEmpty(item.ImageSource) ? ImageHelper.ItemImagePlaceholder : item.ImageSource;
        }

        [ObservableProperty] 
        private string name;

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private string imagePath;

        [RelayCommand]
        private async Task IncreaseQuantity()
        {
            var newQuantityResult = await _itemsService.IncreaseQuantityAsync(_itemId);

            if (newQuantityResult.IsFailed)
            {
                //TODO: add details logging/messaging
                await MessageHelper.ShowErrorAsync("Error in quantity changing! Contact developer");
            }
            else
            {
                Quantity = newQuantityResult.Value;
            }
        }

        [RelayCommand]
        private async Task DecreaseQuantity()
        {
            if (Quantity > 0)
            {
                var newQuantityResult = await _itemsService.DecreaseQuantityAsync(_itemId);

                if (newQuantityResult.IsFailed)
                {
                    //TODO: add details logging/messaging
                    await MessageHelper.ShowErrorAsync("Error in quantity changing! Contact developer");
                }
                else
                {
                    Quantity = newQuantityResult.Value;
                }
            }
        }
    }
}
