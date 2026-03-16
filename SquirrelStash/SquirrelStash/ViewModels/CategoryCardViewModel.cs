using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;

namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        private readonly List<(Item Item, ItemCardViewModel ViewModel)> _allItems;

        public CategoryCardViewModel(Category category, IItemCardViewModelFactory itemCardViewModelFactory)
        {
            Title = category.Title;

            FilterOptions = new ObservableCollection<PropertyDefinition>(
                (category.Properties ?? [])
                    .OrderBy(x => x.Id));
            SelectedFilterAllowedValues = [];
            Items = [];

            _allItems = (category.Items ?? [])
                .Select(item => (item, itemCardViewModelFactory.Create(item)))
                .ToList();
        }

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private PropertyDefinition? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        [ObservableProperty]
        private string? selectedAllowedValue;

        public ObservableCollection<PropertyDefinition> FilterOptions { get; }

        public ObservableCollection<string> SelectedFilterAllowedValues { get; }

        public ObservableCollection<ItemCardViewModel> Items { get; }

        public bool IsAllowedValuesFilter =>
            SelectedFilter?.TypeCode == (int)PropertyTypes.AllowedValues;

        public bool IsManualValueFilter => !IsAllowedValuesFilter;

        [RelayCommand]
        public async Task AddItem()
        {

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
            ApplyFilter();
        }

        partial void OnFilterValueChanged(string? value)
        {
            ApplyFilter();
        }

        partial void OnSelectedAllowedValueChanged(string? value)
        {
            if (FilterValue != value)
            {
                FilterValue = value;
            }
        }

        private void ApplyFilter()
        {
            Items.Clear();

            foreach (var item in _allItems.Where(MatchesFilter).Select(x => x.ViewModel))
            {
                Items.Add(item);
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
    }
}
