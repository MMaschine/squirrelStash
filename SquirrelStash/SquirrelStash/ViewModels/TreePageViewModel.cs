using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.Resources;
using System.Collections.ObjectModel;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Logic;
using SquirrelStash.Views;

namespace SquirrelStash.ViewModels
{
    public partial class TreePageViewModel(
        ICategoryService categoryService,
        ICategoryCardViewModelFactory cardViewModelFactory,
        IEditCategoryDialogFactory editCategoryDialogFactory,
        ILogger<TreePageViewModel> logger) : ObservableObject
    {
        private bool _isInitialized;
        private bool _isLoading;
        private string _searchText = null!;
       
        private readonly List<CategoryCardViewModel> _allCategories = [];

        private ICategoryCardActions CategoryCardActions => new CategoryCardActionsAdapter(ChangeCategoryAsync);

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
                {
                    return;
                }

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized || IsLoading)
            {
                return;
            }

            _isInitialized = true;
            await LoadCategoriesAsync();
        }

        private string[] AllTitles => _allCategories.Select(x => x.Title).ToArray();

        [RelayCommand]
        public async Task CreateCategory()
        {
            var dialogResult = await ShowEditDialogAsync(editCategoryDialogFactory.GetDialogToCreate(AllTitles));

            if (!dialogResult.IsChangesApplied || dialogResult.Data == null)
            {
                if (!string.IsNullOrEmpty(dialogResult.ErrorMessage))
                {
                    logger.LogWarning("Create category dialog failed: {ErrorMessage}", dialogResult.ErrorMessage);
                }

                return;
            }

            var result = await categoryService.CreateCategoryAsync(dialogResult.Data);

            if (result.IsFailed)
            {
                logger.LogError("Create category failed for {CategoryTitle}. Errors: {Errors}",
                    dialogResult.Data.Title,
                    string.Join("; ", result.Errors.Select(x => x.Message)));
                await MessageHelper.ShowErrorAsync(AppText.FailedToAddNewCategory);
            }
            else
            {
                _allCategories.Add(cardViewModelFactory.GetViewModel(result.Value, CategoryCardActions));
                SearchText = string.Empty;
                ApplyFilter(SearchText);

                await MessageHelper.ShowInfoAsync(AppText.FormatCategoryAdded(dialogResult.Data.Title));
            }
        }

        private async Task ChangeCategoryAsync(Category category)
        {
            var dialogResult = await ShowEditDialogAsync(editCategoryDialogFactory.GetDialogToEdit(AllTitles, category));

            await HandleEditCategoryDialogResultAsync(category, dialogResult);
        }

        private async Task HandleEditCategoryDialogResultAsync(Category category, EditCategoryDialogResult dialogResult)
        {
            if (dialogResult.IsDeleted)
            {
                await DeleteCategoryAsync(category);
                return;
            }

            if (!dialogResult.IsChangesApplied || dialogResult.Data == null)
            {
                if (!string.IsNullOrEmpty(dialogResult.ErrorMessage))
                {
                    logger.LogWarning("Edit category dialog failed: {ErrorMessage}", dialogResult.ErrorMessage);
                }

                return;
            }

            await UpdateCategoryAsync(category, dialogResult.Data);
        }

        private async Task UpdateCategoryAsync(Category category, EditCategoryRequest request)
        {
            var result = await categoryService.UpdateCategoryAsync(request);

            if (result.IsFailed)
            {
                await MessageHelper.ShowErrorAsync("ChangeFailed to update category");
                return;
            }

            var currCategory = _allCategories.FirstOrDefault(x => x.CategoryId == category.Id);
            var index = currCategory is null ? -1 : _allCategories.IndexOf(currCategory);

            if (index == -1)
            {
                await MessageHelper.ShowWarningAsync("ChangeFailed to update the category list. Contact the developer");
                logger.LogWarning("Can't update category list. CategoryId: {CategoryId}", category.Id);
                return;
            }


            _allCategories[index] = cardViewModelFactory.GetViewModel(result.Value, CategoryCardActions);
            _allCategories[index].CheckItemWarnings();
            
            ApplyFilter(string.Empty);
        }

        private async Task DeleteCategoryAsync(Category category)
        {
            var result = await categoryService.RemoveCategoryAsync(category.Id);

            if (result.IsFailed)
            {
                logger.LogError("Delete category failed for {CategoryTitle}. Errors: {Errors}",
                    category.Title,
                    string.Join("; ", result.Errors.Select(x => x.Message)));
                await MessageHelper.ShowErrorAsync(AppText.FailedToDeleteCategory);
                return;
            }

            var categoryViewModel = _allCategories.FirstOrDefault(x => x.CategoryId == category.Id);

            if (categoryViewModel == null)
            {
                await MessageHelper.ShowWarningAsync("ChangeFailed to update the category list. Contact the developer");
                logger.LogWarning("Can't delete category from list. CategoryId: {CategoryId}", category.Id);
                return;
            }

            _allCategories.Remove(categoryViewModel);
            ApplyFilter(SearchText);

            await MessageHelper.ShowInfoAsync(AppText.CategoryDeletedMessage);
        }

        private async Task LoadCategoriesAsync()
        {
            IsLoading = true;

            logger.LogInformation("Loading categories.");

            var result = await categoryService.GetCategoriesAsync();

            IsLoading = false;

            if (!result.IsSuccess)
            {
                logger.LogError("Loading categories failed. Errors: {Errors}",
                    string.Join("; ", result.Errors.Select(x => x.Message)));
                await MessageHelper.ShowErrorAsync(AppText.FailedToUploadCategories);
                return;
            }

            _allCategories.AddRange(result.Value.Select(x=> cardViewModelFactory.GetViewModel(x, CategoryCardActions)));
            logger.LogInformation("Loaded {CategoryCount} categories.", result.Value.Count);

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

        private async Task<EditCategoryDialogResult> ShowEditDialogAsync(EditCategoryDialog dialog)
        {
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(dialog);

            return await dialog.ResultTask;
        }

    }
}
