using SquirrelStash.Abstractions;
using SquirrelStash.Resources;

namespace SquirrelStash.Views;

public partial class ConfirmationDialog : ContentPage, IModalDialog<bool>
{
    private readonly TaskCompletionSource<bool> _resultSource = new();
    private bool _actionSelected;

    public Task<bool> ResultTask => _resultSource.Task;

    /// <inheritdoc />
    public Task<bool> DialogResultTask => _resultSource.Task;

    public ConfirmationDialog(string message, string title = "")
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        TitleLabel.Text = string.IsNullOrWhiteSpace(title) ? AppText.AlertConfirmationTitle : title;
        MessageLabel.Text = message;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!_actionSelected)
        {
            _resultSource.TrySetResult(false);
        }
    }

    private async void OnDismissTapped(object? sender, TappedEventArgs e)
    {
        await CompleteAsync(false);
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await CompleteAsync(false);
    }

    private async void OnApplyClicked(object? sender, EventArgs e)
    {
        await CompleteAsync(true);
    }

    private async Task CompleteAsync(bool result)
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
