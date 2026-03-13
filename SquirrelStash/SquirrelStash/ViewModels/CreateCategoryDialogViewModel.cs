using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;
using SquirrelStash.Helpers;
using SquirrelStash.Models;


namespace SquirrelStash.ViewModels
{
    public partial class CreateCategoryDialogViewModel : ObservableObject
    {
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

            if (string.IsNullOrWhiteSpace(trimmedTitle))
            {
                //TODO: to resources
                await MessageHelper.ShowWarningAsync("Title must be set for category");
                return;
            }

            var props = Properties
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Select(x => new CreatePropertyRequest(x.Name.Trim(), x.SelectedType)).ToArray();

            if (!props.Any())
            {   
                //TODO: to resources
                await MessageHelper.ShowWarningAsync("Set at least one property for the Category");
                return;
            }

            var dialogResult =
                DialogResult<CreateCategoryRequest>.GetSuccess(new CreateCategoryRequest(trimmedTitle, props));

            RequestCompleted?.Invoke(dialogResult);
        }
    }
}
