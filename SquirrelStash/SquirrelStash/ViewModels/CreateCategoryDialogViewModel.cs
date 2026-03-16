using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Enums;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using System.Text.RegularExpressions;


namespace SquirrelStash.ViewModels
{
    public partial class CreateCategoryDialogViewModel : ObservableObject
    {
        private static readonly Regex AllowedValuesPattern =
            new(@"^\s*$|^\s*[^,\s]+(?:\s*,\s*[^,\s]+)*\s*$", RegexOptions.Compiled);

        [ObservableProperty]
        private string _title = string.Empty;

        public ObservableCollection<CategoryPropertyViewModel> Properties { get; } = [];

        public event Action<DialogResult<CreateCategoryRequest>>? RequestCompleted;


        [RelayCommand]
        private void AddProperty()
        {
            Properties.Add(new CategoryPropertyViewModel() {DeleteCommand = RemovePropertyCommand});
        }

        [RelayCommand]
        private void RemoveProperty(CategoryPropertyViewModel? property)
        {
            if (property is null)
            {
                return;
            }

            Properties.Remove(property);
        }

        [RelayCommand]
        private void Cancel()
        {
            RequestCompleted?.Invoke(DialogResult<CreateCategoryRequest>.GetCanceled());
        }

        [RelayCommand]
        private async Task Save()
        {
            var trimmedTitle = Title?.Trim() ?? string.Empty;

            var filledProperties = Properties
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
                        : null))
                .ToArray();

            var dialogResult =
                DialogResult<CreateCategoryRequest>.GetSuccess(new CreateCategoryRequest(trimmedTitle, props));

            RequestCompleted?.Invoke(dialogResult);
        }

        private async Task<bool> ValidateCategory(string title, CategoryPropertyViewModel[]? properties)
        {
            //Title is mandatory
            if (string.IsNullOrWhiteSpace(title))
            {
                //TODO: to resources
                await MessageHelper.ShowWarningAsync("Title must be set for category");
                return false;
            }

            //Must be at least one property
            if (properties == null || !properties.Any())
            {
                //TODO: to resources
                await MessageHelper.ShowWarningAsync("Set at least one property for the Category");
                return false;
            }

            //We need to check that the format of the allowed values is correct 
            var invalidProperty = properties.FirstOrDefault(x =>
                x.SelectedType == PropertyTypes.AllowedValues &&
                !AllowedValuesPattern.IsMatch(x.AllowedValues ?? string.Empty));


            if (invalidProperty is not null)
            {
                await MessageHelper.ShowWarningAsync(
                    $"Allowed values for '{invalidProperty.Name.Trim()}' must be comma-separated.");
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
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
