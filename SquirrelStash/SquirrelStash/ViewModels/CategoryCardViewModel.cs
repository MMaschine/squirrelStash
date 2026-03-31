using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Views;

namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        // private readonly List<(Item Item, ItemCardViewModel ViewModel)> _allItems;

        private readonly Category _currentCategory; 

        private readonly IItemsService _itemsService;

        public CategoryCardViewModel(Category category, IItemsService itemService)
        {
            _itemsService = itemService;
            _currentCategory = category;

            Title = category.Title;

            FilterOptions = new ObservableCollection<PropertyDefinition>(
                (category.Properties ?? [])
                    .OrderBy(x => x.Id));
            SelectedFilterAllowedValues = [];

            Items.CollectionChanged += OnItemsCollectionChanged;

            foreach (var item in category.Items)
            {
                Items.Add(new ItemCardViewModel(item, itemService));
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

        public ObservableCollection<PropertyDefinition> FilterOptions { get; }

        public ObservableCollection<string> SelectedFilterAllowedValues { get; }

        public ObservableCollection<ItemCardViewModel> Items { get; } = [];

        public bool IsAllowedValuesFilter =>
            SelectedFilter?.TypeCode == (int)PropertyTypes.AllowedValues;

        public bool IsManualValueFilter => !IsAllowedValuesFilter;

        [RelayCommand]
        public async Task AddItem()
        {
            var dialogResult = await ShowDialogAsync();

            if (!dialogResult.IsSuccess || dialogResult.Data == null)
            {
                if (!string.IsNullOrEmpty(dialogResult.ErrorMessage))
                {
                    //TODO: add logging
                }

                return;
            }

            var result = await _itemsService.AddItemAsync(_currentCategory.Id, dialogResult.Data);

            if (result.IsFailed)
            {
                //TODO: to resources
                //TODO: add logging
                await MessageHelper.ShowErrorAsync("Failed to add item!");
            }
            else
            {
                //TODO: to resources
                await MessageHelper.ShowInfoAsync($"New item added to the category {_currentCategory.Title}");
                Items.Add(new ItemCardViewModel(result.Value, _itemsService));
            }
        }

        partial void OnSelectedFilterChanged(PropertyDefinition? value)
        {
            SelectedAllowedValue = null;
            FilterValue = null;

            SelectedFilterAllowedValues.Clear();

            if (value?.TypeCode == (int)PropertyTypes.AllowedValues)
            {
                foreach (var item in ParseAllowedValues(value.AllowedValues))
                {
                    SelectedFilterAllowedValues.Add(item);
                }
            }

            OnPropertyChanged(nameof(IsAllowedValuesFilter));
            OnPropertyChanged(nameof(IsManualValueFilter));
           // ApplyFilter();
        }

        partial void OnFilterValueChanged(string? value)
        {
          //  ApplyFilter();
        }

        partial void OnSelectedAllowedValueChanged(string? value)
        {
            if (FilterValue != value)
            {
                FilterValue = value;
            }
        }


        private bool MatchesFilter((Item Item, ItemCardViewModel ViewModel) entry)
        {
            if (SelectedFilter is null || string.IsNullOrWhiteSpace(FilterValue))
            {
                return true;
            }

            var expectedValue = FilterValue.Trim();

            return (entry.Item.PropertyEntries ?? [])
                .Any(property =>
                property.PropertyDefinitionId == SelectedFilter.Id &&
                string.Equals(property.Value?.Trim(), expectedValue, StringComparison.OrdinalIgnoreCase));
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
        }

        private async Task<DialogResult<CreateItemRequest>> ShowDialogAsync()
        {
            var dialog = new CreateItemDialog(_currentCategory);

            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            return await dialog.ResultTask;
        }
    }
}
