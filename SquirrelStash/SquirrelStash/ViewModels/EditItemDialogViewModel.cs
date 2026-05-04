using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;
using SquirrelStash.Enums;
using SquirrelStash.Resources;

namespace SquirrelStash.ViewModels
{
    public partial class EditItemDialogViewModel : ObservableObject
    {
        private const int WarningThresholdDef = 5;
        private const int CriticalThresholdDef = 1;

        private readonly int _categoryId;
        private readonly int? _itemId;

        private readonly IImageService _imageService;

        public EditItemDialogViewModel(Category category, IImageService imageService)
        {
            ArgumentNullException.ThrowIfNull(category);

            _imageService = imageService;
            _categoryId = category.Id;

            _itemId = null;
            
            PropertyEntries = new ObservableCollection<CreateItemPropertyEntryViewModel>(
                (category.Properties ?? [])
                    .OrderBy(x => x.Id)
                    .Select(x => new CreateItemPropertyEntryViewModel(x)));
        }

        public EditItemDialogViewModel(Category category, Item item, IImageService imageService) : this(category, imageService)
        {
            _itemId = item.Id;

            ImagePath = item.ImageSource;

            foreach (var entry in item.PropertyEntries)
            {
                var propertyVm = PropertyEntries.FirstOrDefault(x => x.DefinitionId == entry.PropertyDefinitionId);

                if (propertyVm != null)
                {
                    propertyVm.Value = entry.Value;
                }
            }

            MoveMissingValuesToTop();
        }

        public bool IsEdit => _itemId != null;

        public string DialogTitle => IsEdit ? AppText.EditItemPageTitle : AppText.CreateItemPageTitle;

        [ObservableProperty]
        private string? imagePath;

        [ObservableProperty]
        private string warningThreshold = WarningThresholdDef.ToString();

        [ObservableProperty]
        private string criticalThreshold = CriticalThresholdDef.ToString();

        [ObservableProperty]
        private int defaultQuantity = 0;

        public ObservableCollection<CreateItemPropertyEntryViewModel> PropertyEntries { get; }

        public event Action<DialogResult<EditItemRequest>>? RequestCompleted;

        public event Action<CreateItemPropertyEntryViewModel>? PropertyFocusRequested;

        public async Task UpdateImageAsync(ItemImageSource source)
        {
            var result = await _imageService.PickAndStoreImageAsync(source);

            if (result.IsSuccess)
            {
                ImagePath = result.Value;
            }
            else
            {
                await MessageHelper.ShowWarningAsync(AppText.FailedToGetImage);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            RequestCompleted?.Invoke(DialogResult<EditItemRequest>.GetCanceled());
        }

        [RelayCommand]
        private async Task Save()
        {
            var isValid = await ValidateItemAsync();

            if (isValid)
            {
                var request = new EditItemRequest(
                    _categoryId,
                    ImagePath,
                    PropertyEntries
                        .Select(x => new CreatePropertyEntryRequest(x.DefinitionId, x.Value.Trim()))
                        .ToArray(),
                    IsEdit ? _itemId : null,
                    ParseThreshold(WarningThreshold, WarningThresholdDef),
                    ParseThreshold(CriticalThreshold, CriticalThresholdDef),
                    DefaultQuantity >= 0 ? DefaultQuantity : 0);

                RequestCompleted?.Invoke(DialogResult<EditItemRequest>.GetSuccess(request));
            }
        }

        private async Task<bool> ValidateItemAsync()
        {
            var invalidEntry = PropertyEntries.FirstOrDefault(x =>
                string.IsNullOrWhiteSpace(x.Value) ||
                (x.Type == PropertyTypes.AllowedValues) && !x.AllowedValues.Any());

            if (invalidEntry is not null)
            {
                PropertyFocusRequested?.Invoke(invalidEntry);
                await MessageHelper.ShowWarningAsync(
                    string.Format(AppText.ItemValueRequiredFormat, invalidEntry.Name));
                return false;
            }

            return true;
        }

        private int ParseThreshold(string value, int fallback)
        {
            if (!int.TryParse(value, out var parsedValue))
            {
                return fallback;
            }

            return parsedValue;
        }

        private void MoveMissingValuesToTop()
        {
            var orderedEntries = PropertyEntries
                .OrderByDescending(x => x.HasMissingValue)
                .ThenBy(x => x.DefinitionId)
                .ToArray();

            PropertyEntries.Clear();

            foreach (var entry in orderedEntries)
            {
                PropertyEntries.Add(entry);
            }
        }
    }
}
