using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class CreateCategoryDialog : ContentPage
{
    private readonly TaskCompletionSource<DialogResult<CreateCategoryRequest>> _resultSource = new();

    public Task<DialogResult<CreateCategoryRequest>> ResultTask => _resultSource.Task;

    public CreateCategoryDialog(string[] existingTitles)
    {
        InitializeComponent();

        var viewModel = new CreateCategoryDialogViewModel(existingTitles);

        viewModel.RequestCompleted += OnSaveRequested;

        BindingContext = viewModel;
    }

    private async void OnSaveRequested(DialogResult<CreateCategoryRequest> request)
    {
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }
}