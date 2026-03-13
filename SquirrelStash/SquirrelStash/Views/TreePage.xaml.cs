using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class TreePage : ContentPage
{
	private readonly TreePageViewModel _viewModel;

	public TreePage(TreePageViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadCategoriesAsync();
    }
}