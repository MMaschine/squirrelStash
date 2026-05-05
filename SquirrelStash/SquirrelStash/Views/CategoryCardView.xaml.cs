
namespace SquirrelStash.Views;

public partial class CategoryCardView : ContentView
{
	public CategoryCardView()
	{
		InitializeComponent();
	}

    private async void OnClearOrderButtonClicked(object sender, EventArgs e)
    {
        await Task.Yield();

        OrderPicker.Unfocus();

        if (sender is VisualElement clearButton)
        {
            clearButton.Unfocus();
        }
    }
}
