using SquirrelStash.Abstractions;
using SquirrelStash.Enums;

namespace SquirrelStash.Views;

public partial class ItemDetailsDialog : ContentPage, IModalDialog<ItemDetailsDialogResult>
{
    private readonly TaskCompletionSource<ItemDetailsDialogResult> _resultSource = new();
    private bool _actionSelected;

    public Task<ItemDetailsDialogResult> ResultTask => _resultSource.Task;

    /// <inheritdoc />
    public Task<ItemDetailsDialogResult> DialogResultTask => _resultSource.Task;

    public ItemDetailsDialog(string imagePath, string itemName)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        ItemImage.Source = imagePath;
        ItemNameLabel.Text = itemName;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_actionSelected)
        {
            _resultSource.TrySetResult(ItemDetailsDialogResult.None);
        }
    }

    private async void OnDismissTapped(object? sender, TappedEventArgs e)
    {
        await CompleteAsync(ItemDetailsDialogResult.None);
    }

    private async void OnEditClicked(object? sender, EventArgs e)
    {
        await CompleteAsync(ItemDetailsDialogResult.Edit);
    }

    private async void OnCopyClicked(object? sender, EventArgs e)
    {
        await CompleteAsync(ItemDetailsDialogResult.Copy);
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        await CompleteAsync(ItemDetailsDialogResult.Delete);
    }

    private async Task CompleteAsync(ItemDetailsDialogResult result)
    {
        if (_actionSelected)
        {
            return;
        }

        _actionSelected = true;
        await Navigation.PopModalAsync();
        _resultSource.TrySetResult(result);
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
