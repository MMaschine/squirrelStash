using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Resources;
using System.Text.RegularExpressions;

namespace SquirrelStash.ViewModels
{
    public partial class EditCategoryDialogViewModel(string[] existingTitles) : ObservableObject
    {
        private static readonly Regex AllowedValuesPattern =
            new(@"^\s*$|^\s*[^,\s]+(?:\s*,\s*[^,\s]+)*\s*$", RegexOptions.Compiled);

        private readonly string? _initialTitle;
        private readonly int? _categoryId;

        private readonly List<int> _propertiesToRemoveIds = []; 

        public EditCategoryDialogViewModel(string[] existingTitles, Category category)
            : this(existingTitles)
        {
            IsEdit = true;
            _initialTitle = category.Title;
            _categoryId = category.Id;
            Title = category.Title;

            foreach (var property in category.Properties ?? [])
            {
                Properties.Add(new CategoryPropertyViewModel(property, RemovePropertyCommand));
            }
        }

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsEdit { get; } = false;

        public string DialogTitle => IsEdit ? AppText.EditCategoryPageTitle : AppText.CreateCategoryPageTitle;

        public ObservableCollection<CategoryPropertyViewModel> Properties { get; } = [];

        public event Action<DialogResult<EditCategoryRequest>>? RequestCompleted;

        [RelayCommand]
        private void AddProperty()
        {
            Properties.Add(new CategoryPropertyViewModel(RemovePropertyCommand));
        }

        [RelayCommand]
        private void RemoveProperty(CategoryPropertyViewModel? property)
        {
            if (property is null)
            {
                return;
            }

            Properties.Remove(property);

            if (IsEdit && property.Id.HasValue)
            {
                _propertiesToRemoveIds.Add(property.Id.Value);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            RequestCompleted?.Invoke(DialogResult<EditCategoryRequest>.GetCanceled());
        }

        [RelayCommand]
        private async Task Save()
        {
            var trimmedTitle = Title?.Trim() ?? string.Empty;

            if (existingTitles.Any(x => x == trimmedTitle) &&
                (!IsEdit || _initialTitle != trimmedTitle))
            {
                await MessageHelper.ShowErrorAsync(AppText.FormatCategoryExists(trimmedTitle));
                return;
            }

            var filledProperties = (IsEdit? Properties : Properties.Where(x=>x.IsNew))
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .ToArray();

            var isValid = await ValidateCategory(trimmedTitle, filledProperties);

            if (!isValid)
            {
                return;
            }

            var props = filledProperties
                .Select(x => new CreatePropertyRequest(
                    x.Name.Trim(),
                    x.SelectedType,
                    x.SelectedType == PropertyTypes.AllowedValues
                        ? NormalizeAllowedValues(x.AllowedValues)
                        : null, x.Id))
                .ToArray();

            var dialogResult =
                DialogResult<EditCategoryRequest>.GetSuccess(IsEdit ? 
                    new EditCategoryRequest(trimmedTitle,props, _categoryId, _propertiesToRemoveIds.ToArray()) : new EditCategoryRequest(trimmedTitle, props));

            RequestCompleted?.Invoke(dialogResult);
        }

        private async Task<bool> ValidateCategory(string title, CategoryPropertyViewModel[]? properties)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                await MessageHelper.ShowWarningAsync(AppText.CategoryTitleRequired);
                return false;
            }

            if (properties == null || !properties.Any())
            {
                await MessageHelper.ShowWarningAsync(AppText.CategoryPropertyRequired);
                return false;
            }

            var invalidProperty = properties.FirstOrDefault(x =>
                x.SelectedType == PropertyTypes.AllowedValues &&
                (string.IsNullOrWhiteSpace(x.AllowedValues) ||
                !AllowedValuesPattern.IsMatch(x.AllowedValues)));

            if (invalidProperty is not null)
            {
                await MessageHelper.ShowWarningAsync(
                    string.Format(AppText.AllowedValuesInvalidFormat, invalidProperty.Name.Trim()));
                return false;
            }

            return true;
        }

        private string? NormalizeAllowedValues(string? allowedValues)
        {
            if (string.IsNullOrWhiteSpace(allowedValues))
            {
                return null;
            }

            return string.Join(",",
                allowedValues
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase));
        }
    }
}
