using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Abstractions;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Views;


namespace SquirrelStash.ViewModels
{
    public partial class TreePageViewModel(
        ICategoryService categoryService, IItemsService itemsService) : ObservableObject
    {
        private bool _isInitialized;
        private bool _isLoading;
        private string _searchText;

        private List<CategoryCardViewModel> _allCategories { get; } = []; 

        public ObservableCollection<CategoryCardViewModel> Categories { get; } = [];


        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilter(value);
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized || IsLoading)
                return;

            _isInitialized = true;
            await LoadCategoriesAsync();
        }

        [RelayCommand]
        public async Task CreateCategory()
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

            var result = await categoryService.CreateCategoryAsync(dialogResult.Data);

            if (result.IsFailed)
            {
                //TODO: to resources
                //TODO: add logging
                await MessageHelper.ShowErrorAsync("Failed to add new category!");
            }
            else
            {
                _allCategories.Add(new CategoryCardViewModel(result.Value, itemsService));
                SearchText = string.Empty;
                ApplyFilter(SearchText);

                //TODO: to resources
                await MessageHelper.ShowInfoAsync($"Category {dialogResult.Data.Title} added");
            }
        }

        private async Task LoadCategoriesAsync()
        {
            IsLoading = true;

            var result = await categoryService.GetCategoriesAsync();

            IsLoading = false;

            if (!result.IsSuccess)
            {
                //TODO: to resources
                await MessageHelper.ShowErrorAsync("Failed to upload categories");
                return;
            }

            _allCategories.AddRange(result.Value.Select(x=> new CategoryCardViewModel(x,itemsService)));

            ApplyFilter(string.Empty);
        }

        private void ApplyFilter(string filter)
        {
            Categories.Clear();

            var filtered = string.IsNullOrWhiteSpace(filter)
                ? _allCategories
                : _allCategories.Where(x =>
                    !string.IsNullOrWhiteSpace(x.Title) &&
                    x.Title.Contains(filter, StringComparison.OrdinalIgnoreCase));

            foreach (var category in filtered)
            {
                Categories.Add(category);
            }
        }


        private async Task<DialogResult<CreateCategoryRequest>> ShowDialogAsync()
        {
            var dialog = new CreateCategoryDialog();

            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            return await dialog.ResultTask;
        }
    }
}
