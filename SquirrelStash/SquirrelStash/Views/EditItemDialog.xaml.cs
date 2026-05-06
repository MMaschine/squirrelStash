using SquirrelStash.Enums;
using SquirrelStash.Abstractions;
using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class EditItemDialog : ContentPage, IModalDialog<DialogResult<EditItemRequest>>
{
    private readonly TaskCompletionSource<DialogResult<EditItemRequest>> _resultSource = new();

    private readonly EditItemDialogViewModel _viewModel;
    private bool _actionSelected;
    private bool _isDisposed;

    public Task<DialogResult<EditItemRequest>> ResultTask => _resultSource.Task;

    /// <inheritdoc />
    public Task<DialogResult<EditItemRequest>> DialogResultTask => _resultSource.Task;

    public EditItemDialog(EditItemDialogViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _viewModel.RequestCompleted += OnSaveRequested;
        _viewModel.PropertyFocusRequested += OnPropertyFocusRequested;
        BindingContext = _viewModel;
    }

    private async void OnSaveRequested(DialogResult<EditItemRequest> request)
    {
        _actionSelected = true;
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }

    private async void OnImageTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is not EditItemDialogViewModel viewModel)
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Yield();

        var firstMissingProperty = _viewModel.PropertyEntries.FirstOrDefault(x => x.HasMissingValue);

        if (firstMissingProperty is null)
        {
            return;
        }

        PropertyEntriesCollectionView.ScrollTo(
            firstMissingProperty,
            position: ScrollToPosition.MakeVisible,
            animate: false);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_actionSelected)
        {
            _resultSource.TrySetResult(DialogResult<EditItemRequest>.GetCanceled());
        }
    }

    private void OnPropertyFocusRequested(CreateItemPropertyEntryViewModel property)
    {
        MainThread.BeginInvokeOnMainThread(() =>
            PropertyEntriesCollectionView.ScrollTo(
                property,
                position: ScrollToPosition.MakeVisible,
                animate: true));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _viewModel.RequestCompleted -= OnSaveRequested;
        _viewModel.PropertyFocusRequested -= OnPropertyFocusRequested;
        _isDisposed = true;
    }
}
