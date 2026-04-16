using SquirrelStash.Enums;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class CreateItemDialog : ContentPage
{
    private readonly TaskCompletionSource<DialogResult<CreateItemRequest>> _resultSource = new();

    private readonly CreateItemDialogViewModel _viewModel;

    public Task<DialogResult<CreateItemRequest>> ResultTask => _resultSource.Task;

    public CreateItemDialog(CreateItemDialogViewModel viewModel)
    {
        InitializeComponent();

        viewModel.RequestCompleted += OnSaveRequested;

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnSaveRequested(DialogResult<CreateItemRequest> request)
    {
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }

    private async void OnImageTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is not CreateItemDialogViewModel viewModel)
            return;

        var action = await DisplayActionSheet(
            "Choose Photo From",
            "Cancel",
            null,
            "Camera",
            "Gallery");

        if (action == "Camera")
        {
            await _viewModel.UpdateImageAsync(ItemImageSource.Camera);
        }
        else if (action == "Gallery")
        {
            await _viewModel.UpdateImageAsync(ItemImageSource.Gallery);
        }
    }

    private void OnThresholdTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        var text = e.NewTextValue ?? string.Empty;

        if (text.Contains('-'))
        {
            entry.Text = text.Replace("-", string.Empty);
        }
    }
}
