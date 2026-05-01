using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Helpers;
using SquirrelStash.Logic;
using SquirrelStash.Resources;
using SquirrelStash.Views;

namespace SquirrelStash.ViewModels
{
    public partial class ItemCardViewModel : ObservableObject
    {
        private readonly IItemsService _itemsService;
        private readonly Item _item;
        private readonly ILogger<ItemCardViewModel> _logger;

        private Dictionary<int, string> _itemsToOrderBy = [];

        private readonly IItemCardActions _itemCardActions;

        public ItemCardViewModel(
            Item item,
            IItemsService itemService,
            IItemCardActions itemCardActions,
            ILogger<ItemCardViewModel> logger)
        {
            _itemsService = itemService;
            _item = item;
            _logger = logger;
            _itemCardActions = itemCardActions;

            Quantity = item.Quantity;

            Name = string.Join(" ",
                item.PropertyEntries
                    .Select(p => p.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v)));

            HasWarning = string.IsNullOrEmpty(Name);

            ImagePath = string.IsNullOrEmpty(item.ImageSource) ? ImageService.ItemImagePlaceholder : item.ImageSource;

            foreach (var property in item.PropertyEntries)
            {
                _itemsToOrderBy.Add(property.PropertyDefinitionId, property.Value);
            }
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private string imagePath;

        [ObservableProperty]
        private bool hasWarning;

        public int Id => _item.Id;

        public void CheckWarnings(Category category)
        {
            HasWarning = category.Properties.Count != _itemsToOrderBy.Count || string.IsNullOrEmpty(Name);
        }

        public string GetOrderByValue(int id)
        {
            return _itemsToOrderBy.TryGetValue(id, out var result) ? result : string.Empty;
        }

        [RelayCommand]
        private async Task EditItem()
        {
            await _itemCardActions.EditItemAsync(_item);
        }

        [RelayCommand]
        private async Task ShowItemDetails()
        {
            var dialog = new ItemDetailsDialog(ImagePath, Name);
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            var action = await dialog.ResultTask;

            switch (action)
            {
                case ItemDetailsDialogResult.Edit:
                    await _itemCardActions.EditItemAsync(_item);
                    break;
                case ItemDetailsDialogResult.Copy:
                    await _itemCardActions.CopyItemAsync(_item);
                    break;
                case ItemDetailsDialogResult.Delete:
                    await _itemCardActions.DeleteItemAsync(_item);
                    break;
            }
        }

        [RelayCommand]
        private async Task IncreaseQuantity()
        {
            var newQuantityResult = await _itemsService.IncreaseQuantityAsync(_item.Id);

            if (newQuantityResult.IsFailed)
            {
                _logger.LogError($"Increase quantity failed for item {_item.Id}. Errors: {string.Join("; ", newQuantityResult.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.QuantityChangeError);
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
                var newQuantityResult = await _itemsService.DecreaseQuantityAsync(_item.Id);

                if (newQuantityResult.IsFailed)
                {
                    _logger.LogError($"Decrease quantity failed for item {_item.Id}. Errors: {string.Join("; ", newQuantityResult.Errors.Select(x => x.Message))}");
                    await MessageHelper.ShowErrorAsync(AppText.QuantityChangeError);
                }
                else
                {
                    Quantity = newQuantityResult.Value;
                }
            }
        }
    }
}
