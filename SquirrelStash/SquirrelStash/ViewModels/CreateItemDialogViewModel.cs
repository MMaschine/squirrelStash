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
    public partial class CreateItemDialogViewModel : ObservableObject
    {
        private const int WarningThresholdDef = 5;
        private const int CriticalThresholdDef = 1;

        private readonly int _categoryId;

        private readonly IImageService _imageService;

        public CreateItemDialogViewModel(Category category, IImageService imageService)
        {
            ArgumentNullException.ThrowIfNull(category);

            _imageService = imageService;
            _categoryId = category.Id;

            PropertyEntries = new ObservableCollection<CreateItemPropertyEntryViewModel>(
                (category.Properties ?? [])
                    .OrderBy(x => x.Id)
                    .Select(x => new CreateItemPropertyEntryViewModel(x)));
        }

        [ObservableProperty]
        private string? imagePath;

        [ObservableProperty]
        private string warningThreshold = WarningThresholdDef.ToString();

        [ObservableProperty]
        private string criticalThreshold = CriticalThresholdDef.ToString();

        [ObservableProperty]
        private int defaultQuantity = 0;

        public ObservableCollection<CreateItemPropertyEntryViewModel> PropertyEntries { get; }

        public event Action<DialogResult<CreateItemRequest>>? RequestCompleted;

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
            RequestCompleted?.Invoke(DialogResult<CreateItemRequest>.GetCanceled());
        }

        [RelayCommand]
        private async Task Save()
        {
            var isValid = await ValidateItemAsync();

            if (isValid)
            {
                var request = new CreateItemRequest(
                    _categoryId,
                    ImagePath,
                    PropertyEntries
                        .Select(x => new CreatePropertyEntryRequest(x.DefinitionId, x.Value.Trim()))
                        .ToArray(),
                    ParseThreshold(WarningThreshold, WarningThresholdDef),
                    ParseThreshold(CriticalThreshold, CriticalThresholdDef),
                    DefaultQuantity >= 0 ? DefaultQuantity : 0);

                RequestCompleted?.Invoke(DialogResult<CreateItemRequest>.GetSuccess(request));
            }
        }

        private async Task<bool> ValidateItemAsync()
        {
            var invalidEntry = PropertyEntries.FirstOrDefault(x =>
                string.IsNullOrWhiteSpace(x.Value) ||
                (x.Type == PropertyTypes.AllowedValues) && !x.AllowedValues.Any());

            if (invalidEntry is not null)
            {
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
    }
}
