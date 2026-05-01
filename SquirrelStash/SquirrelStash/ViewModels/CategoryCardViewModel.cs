using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Resources;
using SquirrelStash.Logic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SquirrelStash.Enums;


namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        private readonly Category _currentCategory;
        private readonly IItemsService _itemsService;
        private readonly ILogger _logger;
        private readonly IItemCardViewModelFactory _itemCardViewModelFactory;
        private readonly IEditItemDialogFactory _editItemDialogFactory;
        private readonly ICategoryCardActions _categoryCardActions;
        private readonly IItemCardActions _itemCardActions;

        public CategoryCardViewModel(
            Category category,
            IItemsService itemService,
            IItemCardViewModelFactory itemCardViewModelFactory,
            IEditItemDialogFactory editItemDialogFactory,
            ICategoryCardActions categoryCardActions,
            ILogger<CategoryCardViewModel> logger)
        {
            _itemsService = itemService;
            _currentCategory = category;
            _itemCardViewModelFactory = itemCardViewModelFactory;
            _editItemDialogFactory = editItemDialogFactory;
            _categoryCardActions = categoryCardActions;
            _itemCardActions = new ItemCardActionsAdapter(EditItemAsync, DeleteItemAsync, CopyItemAsync);
            _logger = logger;

            Title = category.Title;

            OrderOptions = new ObservableCollection<PropertyDefinition>(
                (category.Properties ?? []));

            Items.CollectionChanged += OnItemsCollectionChanged;

            foreach (var item in category.Items)
            {
                Items.Add(_itemCardViewModelFactory.GetViewModel(item, _itemCardActions));
            }
        }

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private PropertyDefinition? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        [ObservableProperty]
        private int itemsCount;

        [ObservableProperty]
        private bool isItemsVisible;

        [ObservableProperty]
        private PropertyDefinition? selectedOrderOption;

        public int CategoryId => _currentCategory.Id;

        public string ItemsHeaderText => AppText.FormatItemsHeader(ItemsCount);

        public bool CanOrderItems => ItemsCount >= 2;

        public ObservableCollection<PropertyDefinition> OrderOptions { get; }

        public ObservableCollection<ItemCardViewModel> Items { get; private set; } = [];

        public void CheckItemWarnings()
        {
            foreach (var item in Items)
            {
                item.CheckWarnings(_currentCategory);
            }
        }

        [RelayCommand]
        private void ToggleItemsVisibility()
        {
            IsItemsVisible = !IsItemsVisible;
        }

        [RelayCommand]
        private async Task EditCategory()
        {
            await _categoryCardActions.EditCategoryAsync(_currentCategory);
        }

        [RelayCommand]
        private async Task AddItem()
        {
            var dialogResult = await ShowDialogToAddItemAsync();

            if (!dialogResult.IsSuccess || dialogResult.Data == null)
            {
                if (!string.IsNullOrEmpty(dialogResult.ErrorMessage))
                {
                    _logger.LogWarning($"Create item dialog failed for category {Title}: {dialogResult.ErrorMessage}",
                        _currentCategory.Title,
                        dialogResult.ErrorMessage);
                }

                return;
            }

            var result = await _itemsService.AddItemAsync(dialogResult.Data);

            if (result.IsFailed)
            {  
                _logger.LogError($"Add item failed for category {_currentCategory.Title}. Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.FailedToAddItem);
            }
            else
            {
                await MessageHelper.ShowInfoAsync(AppText.FormatItemAdded(_currentCategory.Title));
                Items.Add(_itemCardViewModelFactory.GetViewModel(result.Value, _itemCardActions));
                IsItemsVisible = true;

                //If we have order by function, we should apply it 
                if (SelectedOrderOption != null)
                {
                    SortItems(SelectedOrderOption);
                }
            }
        }

        
        [RelayCommand]
        private void HandleOrderSelection(PropertyDefinition? orderOption)
        {
            if (orderOption != null)
            {
                SortItems(orderOption);
            }
        }

        [RelayCommand]
        private void ClearOrderSelection()
        {
            SelectedOrderOption = null;
            SortItemsById();
        }

        partial void OnSelectedOrderOptionChanged(PropertyDefinition? value)
        {
            HandleOrderSelectionCommand.Execute(value);
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCount = Items.Count;
            OnPropertyChanged(nameof(ItemsHeaderText));
            OnPropertyChanged(nameof(CanOrderItems));
        }

        private async Task<DialogResult<EditItemRequest>> ShowDialogToAddItemAsync()
        {
            var dialog = _editItemDialogFactory.CreateDialog(_currentCategory);

            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            return await dialog.ResultTask;
        }

        private async Task<DialogResult<EditItemRequest>> ShowDialogToEditAsync(Item item)
        {
            var dialog = _editItemDialogFactory.CreateDialog(_currentCategory, item);

            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);
            return await dialog.ResultTask;
        }

        private async Task EditItemAsync(Item item)
        {
            var dialogResult = await ShowDialogToEditAsync(item);

            if (!dialogResult.IsSuccess || dialogResult.Data == null)
            {
                if (!string.IsNullOrEmpty(dialogResult.ErrorMessage))
                {
                    _logger.LogWarning($"Edit item dialog failed for category {Title} and item with id: {item.Id}: {dialogResult.ErrorMessage}",
                        _currentCategory.Title,
                        dialogResult.ErrorMessage);
                }

                return;
            }

            var result = await _itemsService.UpdateItemAsync( dialogResult.Data);

            if (result.IsFailed)
            {
                _logger.LogError($"Update item failed for category {_currentCategory.Title}. Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.FailedToUpdateItem);
            }
            else
            {
                var newItemVm = _itemCardViewModelFactory.GetViewModel(result.Value, _itemCardActions);
                newItemVm.CheckWarnings(_currentCategory);

                var currVm = Items.FirstOrDefault(x => x.Id == newItemVm.Id);

                if (currVm != null)
                {
                    var index = Items.IndexOf(currVm);
                    Items[index] = newItemVm;
                    await MessageHelper.ShowInfoAsync(AppText.FormatItemUpdate(_currentCategory.Title, newItemVm.Name));
                    IsItemsVisible = true;
                }
                else
                {
                    //If we don't have VM in the list it is exceptional situation - log and notify user
                    await MessageHelper.NotifyException(new InvalidOperationException("Failed to update Items list"),
                        "Failed to update Items list", _logger);
                }

                //If we have order by function, we should apply it 
                if (SelectedOrderOption != null)
                {
                    SortItems(SelectedOrderOption);
                }
            }
        }

        private async Task DeleteItemAsync(Item item)
        {
            var itemName = GetItemName(item);
            var confirmed = await MessageHelper.ShowConfirmationAsync(AppText.FormatDeleteItemConfirmation(itemName));

            if (!confirmed)
            {
                return;
            }

            var result = await _itemsService.RemoveItemAsync(item.Id);

            if (result.IsFailed)
            {
                _logger.LogError($"Delete item failed for category {_currentCategory.Title} and item {item.Id}. Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.FailedToDeleteItem);
                return;
            }

            var itemViewModel = Items.FirstOrDefault(x => x.Id == item.Id);

            if (itemViewModel == null)
            {
                await MessageHelper.NotifyException(new InvalidOperationException("Failed to delete item from Items list"),
                    "Failed to delete item from Items list", _logger);
                return;
            }

            Items.Remove(itemViewModel);
            await MessageHelper.ShowInfoAsync(AppText.ItemDeletedMessage);
        }

        private async Task CopyItemAsync(Item item)
        {
            var request = new EditItemRequest(
                _currentCategory.Id,
                item.ImageSource,
                item.PropertyEntries
                    .Select(x => new CreatePropertyEntryRequest(x.PropertyDefinitionId, x.Value))
                    .ToArray(),
                null,
                item.WarningThreshold,
                item.CriticalThreshold,
                item.Quantity,
                item.Note ?? string.Empty);

            var result = await _itemsService.AddItemAsync(request);

            if (result.IsFailed)
            {
                _logger.LogError($"Copy item failed for category {_currentCategory.Title} and item {item.Id}. Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.FailedToCopyItem);
                return;
            }

            var copiedItemViewModel = _itemCardViewModelFactory.GetViewModel(result.Value, _itemCardActions);
            Items.Add(copiedItemViewModel);
            IsItemsVisible = true;

            if (SelectedOrderOption != null)
            {
                SortItems(SelectedOrderOption);
            }

            await EditItemAsync(result.Value);
        }

        private static string GetItemName(Item item)
        {
            var itemName = string.Join(" ",
                item.PropertyEntries
                    .Select(p => p.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v)));

            return string.IsNullOrWhiteSpace(itemName) ? item.Id.ToString() : itemName;
        }

        private void SortItems(PropertyDefinition property)
        {
            //If the property is "AllowedValues" we should place them according the order defined in the values string ,
            //not default order (Size: S, M, L, XL not L,M,S,XL)
            if (property.TypeCode == (int)PropertyTypes.AllowedValues && 
                !string.IsNullOrWhiteSpace(property.AllowedValues))
            {
                var orderMap = property.AllowedValues.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select((value, index) => (value, index))
                    .ToDictionary(x => x.value, x => x.index, StringComparer.OrdinalIgnoreCase);

                MoveItemsToOrder(Items
                    .OrderBy(x => orderMap.TryGetValue(x.GetOrderByValue(property.Id), out var index) ? index : int.MaxValue));
            }
            else
            {
                if (property.TypeCode == (int)PropertyTypes.Numeric) //Beware of numeric values 
                {
                    MoveItemsToOrder(Items.OrderBy(x =>
                        Int32.TryParse(x.GetOrderByValue(property.Id), out var index) ? index : int.MaxValue)); 
                }
                else
                {
                    MoveItemsToOrder(Items.OrderBy(x => x.GetOrderByValue(property.Id)));
                }
            }
        }

        private void SortItemsById()
        {
            MoveItemsToOrder(Items.OrderBy(x => x.Id));
        }

        private void MoveItemsToOrder(IEnumerable<ItemCardViewModel> orderedItems)
        {
            var ordered = orderedItems.ToArray();
            for (var i = 0; i < ordered.Length; i++)
            {
                var oldIndex = Items.IndexOf(ordered[i]);
                if (oldIndex != i)
                {
                    Items.Move(oldIndex, i);
                }
            }
        }
    }
}
