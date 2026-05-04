using SquirrelStash.Models;
using SquirrelStash.Requests;
using SquirrelStash.ViewModels;
using System.Collections.Specialized;

namespace SquirrelStash.Views;

public partial class EditCategoryDialog : ContentPage
{
    private readonly TaskCompletionSource<EditCategoryDialogResult> _resultSource = new();
    private readonly EditCategoryDialogViewModel _viewModel;

    public Task<EditCategoryDialogResult> ResultTask => _resultSource.Task;

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

        _viewModel.RequestCompleted -= OnSaveRequested;
        _viewModel.Properties.CollectionChanged -= OnPropertiesCollectionChanged;
    }
}
