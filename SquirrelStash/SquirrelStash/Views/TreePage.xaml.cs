using SquirrelStash.ViewModels;
using Microsoft.Maui.ApplicationModel;

namespace SquirrelStash.Views;

public partial class TreePage : ContentPage
{
	private readonly TreePageViewModel _viewModel;

	public TreePage(TreePageViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
        var version = AppInfo.Current.VersionString;
        VersionLabel.Text = version.Equals("0.1-alpha.1", StringComparison.OrdinalIgnoreCase)
            ? "Version 0.1 Alpha 1"
            : $"Version {version}";
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = _viewModel.InitializeAsync();
    }
}
