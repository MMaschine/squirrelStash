using SquirrelStash.ViewModel;

namespace SquirrelStash.Views;

public partial class CategoryCardView : ContentView
{
	public CategoryCardView()
	{
		InitializeComponent();

		//Todo: temp 
        BindingContext = new CategoryCardViewModel(); 
    }
}