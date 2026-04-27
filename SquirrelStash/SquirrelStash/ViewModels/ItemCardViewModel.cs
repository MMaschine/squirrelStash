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
        private readonly int _itemId;
        private readonly ILogger<ItemCardViewModel> _logger;

        private Dictionary<int, string> _itemsToOrderBy = [];

        public ItemCardViewModel(Item item, IItemsService itemService, ILogger<ItemCardViewModel> logger)
        {
            _itemsService = itemService;
            _itemId = item.Id;
            _logger = logger;

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

        public int Id => _itemId;

        public void CheckWarnings(Category category)
        {
            HasWarning = category.Properties.Count != _itemsToOrderBy.Count || string.IsNullOrEmpty(Name);
        }

        public string GetOrderByValue(int id)
        {
            return _itemsToOrderBy.TryGetValue(id, out var result) ? result : string.Empty;
        } 

        [RelayCommand]
        private async Task IncreaseQuantity()
        {
            var newQuantityResult = await _itemsService.IncreaseQuantityAsync(_itemId);

            if (newQuantityResult.IsFailed)
            {
                _logger.LogError($"Increase quantity failed for item {_itemId}. Errors: {string.Join("; ", newQuantityResult.Errors.Select(x => x.Message))}");
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
                var newQuantityResult = await _itemsService.DecreaseQuantityAsync(_itemId);

                if (newQuantityResult.IsFailed)
                {
                    _logger.LogError($"Decrease quantity failed for item {_itemId}. Errors: {string.Join("; ", newQuantityResult.Errors.Select(x => x.Message))}");
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
