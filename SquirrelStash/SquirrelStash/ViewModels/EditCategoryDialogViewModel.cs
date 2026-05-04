using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Resources;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using FluentResults;

namespace SquirrelStash.ViewModels
{
    public partial class EditCategoryDialogViewModel(string[] existingTitles) : ObservableObject
    {
        private static readonly Regex AllowedValuesPattern =
            new(
                @"^\s*$|^(?!.*(?:^|,)\s*([^,\s]+)\s*(?=,|$).*(?:^|,)\s*\1\s*(?=,|$))\s*[^,\s]+(?:\s*,\s*[^,\s]+)*\s*$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

        public bool CanRemoveCategory => IsEdit;

        public ObservableCollection<CategoryPropertyViewModel> Properties { get; } = [];

        public event Action<EditCategoryDialogResult>? RequestCompleted;

        [RelayCommand]
        private void AddProperty()
        {
            Properties.Add(new CategoryPropertyViewModel(RemovePropertyCommand));
            SaveCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void RemoveProperty(CategoryPropertyViewModel? property)
        {
            if (property is null)
            {
                return;
            }

            Properties.Remove(property);
            SaveCommand.NotifyCanExecuteChanged();

            if (IsEdit && property.Id.HasValue)
            {
                _propertiesToRemoveIds.Add(property.Id.Value);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            RequestCompleted?.Invoke(EditCategoryDialogResult.GetCanceled());
        }

        [RelayCommand]
        private async Task RemoveCategory()
        {
            if (!IsEdit)
            {
                return;
            }

            var confirmed = await MessageHelper.ShowConfirmationAsync(AppText.FormatDeleteCategoryConfirmation(Title));

            if (confirmed)
            {
                var res = EditCategoryDialogResult.GetDeleted();
                RequestCompleted?.Invoke(res);
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {

            var validTitleResult = await GetTrimmedValidTitleAsync();
            if (!validTitleResult.IsSuccess)
            {
                return;
            }

            var trimmedTitle = validTitleResult.Value;

            //After passing this all properties are valid 
            if (!(await ValidatePropertiesAsync()).IsSuccess)
            {
                return;
            }


            //New category - add all properties, updating category - only that not in the DB
            var propertiesToAdd = (IsEdit ? Properties.Where(x => x.IsNew) : Properties)
                .Select(x => new CreatePropertyRequest(
                    x.Name.Trim(),
                    x.SelectedType,
                    x.SelectedType == PropertyTypes.AllowedValues
                        ? NormalizeAllowedValues(x.AllowedValues)
                        : null, x.Id))
                .ToArray();

            var dialogResult =
                EditCategoryDialogResult.GetChangesApplied(IsEdit ? 
                    new EditCategoryRequest(trimmedTitle, propertiesToAdd, _categoryId, _propertiesToRemoveIds.ToArray()) :
                    new EditCategoryRequest(trimmedTitle, propertiesToAdd));

            RequestCompleted?.Invoke(dialogResult);
        }

        private async Task<Result<string>> GetTrimmedValidTitleAsync()
        {
            var trimmedTitle = Title?.Trim();

            //Category must have a title
            if (string.IsNullOrWhiteSpace(trimmedTitle))
            {
                await MessageHelper.ShowErrorAsync(AppText.CategoryTitleRequired);
                return Result.Fail("Empty title");
            }

            //Title must be unique
            if (existingTitles.Any(x => x == trimmedTitle) &&
                (!IsEdit || _initialTitle != trimmedTitle))
            {
                await MessageHelper.ShowErrorAsync(AppText.FormatCategoryExists(trimmedTitle));
                return Result.Fail("Not unique title");
            }

            return Result.Ok(trimmedTitle);
        }

        private async Task<Result> ValidatePropertiesAsync()
        {
            //Must be at least one property
            if (!Properties.Any())
            {
                await MessageHelper.ShowErrorAsync(AppText.CategoryPropertyRequired);
                return Result.Fail("No properties");
            }

            var namelessProperty = Properties.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Name));

            if (namelessProperty != null)
            {
                await MessageHelper.ShowErrorAsync(AppText.FillPropertyName);
                return Result.Fail("Nameless property");
            }

            var invalidProperty = Properties.FirstOrDefault(x =>
                x.SelectedType == PropertyTypes.AllowedValues &&
                (string.IsNullOrWhiteSpace(x.AllowedValues) ||
                 !AllowedValuesPattern.IsMatch(x.AllowedValues)));

            if (invalidProperty is not null)
            {
                await MessageHelper.ShowErrorAsync(
                    string.Format(AppText.AllowedValuesInvalidFormat, invalidProperty.Name.Trim()));
                return Result.Fail("Allowed values format violation");
            }

            return Result.Ok();
        }


        private bool CanSave()
        {
            return Properties.Any();
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
