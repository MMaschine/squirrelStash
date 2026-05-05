using SquirrelStash.Models;
using SquirrelStash.ViewModels;
using System.Collections.Specialized;
using SquirrelStash.Abstractions;

namespace SquirrelStash.Views;

public partial class EditCategoryDialog : ContentPage, IModalDialog<EditCategoryDialogResult>
{
    private readonly TaskCompletionSource<EditCategoryDialogResult> _resultSource = new();

    private readonly EditCategoryDialogViewModel _viewModel;
    private bool _actionSelected;
    private bool _isDisposed;

    /// <inheritdoc />
    public Task<EditCategoryDialogResult> DialogResultTask => _resultSource.Task;

    public EditCategoryDialog(EditCategoryDialogViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _viewModel.RequestCompleted += OnSaveRequested;
        _viewModel.Properties.CollectionChanged += OnPropertiesCollectionChanged;

        BindingContext = _viewModel;
    }

    private async void OnSaveRequested(EditCategoryDialogResult request)
    {
        _actionSelected = true;
        _resultSource.TrySetResult(request);
        await Navigation.PopModalAsync();
    }

    private async void OnPropertiesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems is not { Count: > 0 })
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Task.Yield();
            PropertiesCollectionView.ScrollTo(
                _viewModel.Properties.Count - 1,
                position: ScrollToPosition.MakeVisible,
                animate: true);
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_actionSelected)
        {
            _resultSource.TrySetResult(EditCategoryDialogResult.GetCanceled());
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _viewModel.RequestCompleted -= OnSaveRequested;
        _viewModel.Properties.CollectionChanged -= OnPropertiesCollectionChanged;
        _isDisposed = true;
    }
}
