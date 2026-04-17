using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Logic.Factories;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Resources;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        private readonly Category _currentCategory;
        private readonly IItemsService _itemsService;
        private readonly ILogger _logger;
        private readonly IItemCardViewModelFactory _itemCardViewModelFactory;
        private readonly ICreateItemDialogFactory _createItemDialogFactory;

        public CategoryCardViewModel(
            Category category,
            IItemsService itemService,
            IItemCardViewModelFactory itemCardViewModelFactory,
            ICreateItemDialogFactory createItemDialogFactory,
            ILogger<CategoryCardViewModel> logger)
        {
            _itemsService = itemService;
            _currentCategory = category;
            _itemCardViewModelFactory = itemCardViewModelFactory;
            _createItemDialogFactory = createItemDialogFactory;
            _logger = logger;

            Title = category.Title;

            OrderOptions = new ObservableCollection<PropertyDefinition>(
                (category.Properties ?? []));

            Items.CollectionChanged += OnItemsCollectionChanged;

            foreach (var item in category.Items)
            {
                Items.Add(_itemCardViewModelFactory.GetViewModel(item));
            }
        }

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private PropertyDefinition? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        [ObservableProperty]
        private string? selectedAllowedValue;

        [ObservableProperty]
        private int itemsCount;

        [ObservableProperty]
        private bool isItemsVisible;

        [ObservableProperty]
        private string itemsToggleText = ">";

        [ObservableProperty]
        private PropertyDefinition? selectedOrderOption;

        public string ItemsHeaderText => AppText.FormatItemsHeader(ItemsCount);

        public ObservableCollection<PropertyDefinition> OrderOptions { get; }

        public ObservableCollection<ItemCardViewModel> Items { get; private set; } = [];



        [RelayCommand]
        private void ToggleItemsVisibility()
        {
            IsItemsVisible = !IsItemsVisible;
        }

        [RelayCommand]
        public async Task AddItem()
        {
            var dialogResult = await ShowDialogAsync();

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

            var result = await _itemsService.AddItemAsync(_currentCategory.Id, dialogResult.Data);

            if (result.IsFailed)
            {  
                _logger.LogError($"Add item failed for category {_currentCategory.Title}. Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                await MessageHelper.ShowErrorAsync(AppText.FailedToAddItem);
            }
            else
            {
                await MessageHelper.ShowInfoAsync(AppText.FormatItemAdded(_currentCategory.Title));
                Items.Add(_itemCardViewModelFactory.GetViewModel(result.Value));
                IsItemsVisible = true;

                //If we have order by function, we should apply it 
                if (SelectedOrderOption != null)
                {
                    SortItems(SelectedOrderOption.Id);
                }
            }
        }

        [RelayCommand]
        private void HandleOrderSelection(PropertyDefinition? orderOption)
        {
            if (orderOption != null)
            {
                SortItems(orderOption.Id);
            }
        }

        [RelayCommand]
        private void ClearOrderSelection()
        {
            SelectedOrderOption = null;
            SortItemsById();
        }

        partial void OnSelectedAllowedValueChanged(string? value)
        {
            if (FilterValue != value)
            {
                FilterValue = value;
            }
        }

        partial void OnSelectedOrderOptionChanged(PropertyDefinition? value)
        {
            HandleOrderSelectionCommand.Execute(value);
        }

        partial void OnIsItemsVisibleChanged(bool value)
        {
            ItemsToggleText = value ? "v" : ">";
        }

        private static IEnumerable<string> ParseAllowedValues(string? allowedValues)
        {
            if (string.IsNullOrWhiteSpace(allowedValues))
            {
                return [];
            }

            return allowedValues
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCount = Items.Count;
            OnPropertyChanged(nameof(ItemsHeaderText));
        }

        private async Task<DialogResult<CreateItemRequest>> ShowDialogAsync()
        {
            var dialog = _createItemDialogFactory.CreateDialog(_currentCategory);

            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            return await dialog.ResultTask;
        }

        private void SortItems(int id)
        {
            MoveItemsToOrder(Items.OrderBy(x => x.GetOrderByValue(id)));
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
