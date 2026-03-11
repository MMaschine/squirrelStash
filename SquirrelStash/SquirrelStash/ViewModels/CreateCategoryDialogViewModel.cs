using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Requests;
using System.Collections.ObjectModel;

namespace SquirrelStash.ViewModels
{
    public partial class CreateCategoryDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        public ObservableCollection<CategoryPropertyViewModel> Properties { get; } = [];

        public event Action? CancelRequested;

        public event Action<CreateCategoryRequest>? SaveRequested;

        public CreateCategoryDialogViewModel()
        {
        }

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
            CancelRequested?.Invoke();
        }

        [RelayCommand]
        private void Save()
        {
            var trimmedTitle = Title?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(trimmedTitle))
            {
                return;
            }

            var props = Properties
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Select(x => new CreatePropertyRequest(x.Name.Trim(), x.SelectedType)).ToArray();

            SaveRequested?.Invoke(new CreateCategoryRequest(trimmedTitle, props));
        }
    }
}
