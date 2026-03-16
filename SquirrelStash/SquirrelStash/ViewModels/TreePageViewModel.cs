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
        public ObservableCollection<CategoryCardViewModel> Categories { get; } = [];

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
                //TODO: to resources
                await MessageHelper.ShowInfoAsync($"Category {dialogResult.Data.Title} added");
            }
        }

        public async Task LoadCategoriesAsync()
        {
            var result = await categoryService.GetCategoriesAsync();

            if (!result.IsSuccess)
            {
                //TODO: to resources
                await MessageHelper.ShowErrorAsync("Failed to upload categories");
                return;
            }

            Categories.Clear();

            foreach (var category in result.Value)
            {
                Categories.Add(new CategoryCardViewModel(category, itemsService));
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
