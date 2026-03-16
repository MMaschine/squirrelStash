using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class CreateItemDialog : ContentPage
{
    private readonly TaskCompletionSource<DialogResult<CreateItemRequest>> _resultSource = new();

    public Task<DialogResult<CreateItemRequest>> ResultTask => _resultSource.Task;

    public CreateItemDialog(Category category)
    {
        InitializeComponent();

        var viewModel = new CreateItemDialogViewModel(category);
        viewModel.RequestCompleted += OnSaveRequested;

        BindingContext = viewModel;
    }

    private async void OnSaveRequested(DialogResult<CreateItemRequest> request)
    {
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }
}
