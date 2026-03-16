using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;

namespace SquirrelStash.ViewModels
{
    public partial class CreateItemDialogViewModel : ObservableObject
    {
        private readonly int _categoryId;

        public CreateItemDialogViewModel(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

            _categoryId = category.Id;

            PropertyEntries = new ObservableCollection<CreateItemPropertyEntryViewModel>(
                (category.Properties ?? [])
                    .OrderBy(x => x.Id)
                    .Select(x => new CreateItemPropertyEntryViewModel(x)));
        }

        [ObservableProperty]
        private string? imageSource;

        public ObservableCollection<CreateItemPropertyEntryViewModel> PropertyEntries { get; }

        public event Action<DialogResult<CreateItemRequest>>? RequestCompleted;

        [RelayCommand]
        private async Task PickImage()
        {
            //Add image logic 

            //try
            //{
            //    var result = await FilePicker.Default.PickAsync(new PickOptions
            //    {
            //        PickerTitle = "Select product image",
            //        FileTypes = FilePickerFileType.Images
            //    });

            //    if (result is not null)
            //    {
            //        ImageSource = result.FullPath;
            //    }
            //}
            //catch (Exception)
            //{
            //    await MessageHelper.ShowWarningAsync("Unable to select image.");
            //}
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
                    ImageSource,
                    PropertyEntries
                        .Select(x => new CreatePropertyEntryRequest(x.DefinitionId, x.Value.Trim()))
                        .ToArray());

                RequestCompleted?.Invoke(DialogResult<CreateItemRequest>.GetSuccess(request));
            }
        }

        private async Task<bool> ValidateItemAsync()
        {
            var invalidEntry = PropertyEntries.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Value));

            if (invalidEntry is not null)
            {
                await MessageHelper.ShowWarningAsync(
                    $"Set a value for property '{invalidEntry.Name}'.");
                return false;
            }

            return true;
        }
    }
}
