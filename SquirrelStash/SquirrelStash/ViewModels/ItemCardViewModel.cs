using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Logic;
using SquirrelStash.Resources;

namespace SquirrelStash.ViewModels
{
    public partial class ItemCardViewModel : ObservableObject
    {
        private readonly IItemsService _itemsService;
        private readonly Item _item;
        private readonly ILogger<ItemCardViewModel> _logger;

        private Dictionary<int, string> _itemsToOrderBy = [];

        private readonly Func<Item, Task> _editAction;  

        public ItemCardViewModel(Item item, IItemsService itemService, Func<Item,Task> editAction, ILogger<ItemCardViewModel> logger)
        {
            _itemsService = itemService;
            _item = item;
            _logger = logger;
            _editAction = editAction;

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
            await _editAction.Invoke(_item);
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
