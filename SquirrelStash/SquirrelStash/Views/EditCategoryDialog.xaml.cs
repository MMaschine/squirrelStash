using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class EditCategoryDialog : ContentPage
{
    private readonly TaskCompletionSource<DialogResult<EditCategoryRequest>> _resultSource = new();

    public Task<DialogResult<EditCategoryRequest>> ResultTask => _resultSource.Task;

    public EditCategoryDialog(EditCategoryDialogViewModel viewModel)
    {
        InitializeComponent();

        viewModel.RequestCompleted += OnSaveRequested;

        BindingContext = viewModel;
    }

    private async void OnSaveRequested(DialogResult<EditCategoryRequest> request)
    {
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }
}
