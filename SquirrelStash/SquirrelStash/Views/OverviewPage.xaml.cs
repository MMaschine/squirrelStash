using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class OverviewPage : ContentPage
{
    private readonly OverviewPageViewModel _viewModel;

    public OverviewPage(OverviewPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = _viewModel.LoadOverviewAsync();
    }
}
