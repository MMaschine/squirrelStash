using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class TreePage : ContentPage
{
	public TreePage(TreePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}