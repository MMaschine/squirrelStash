using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class CategoryCardView : ContentView
{
	public CategoryCardView()
	{
		InitializeComponent();
        SetFilterVisibility(false);
    }

    private void OpenFilterOnClicked(object? sender, EventArgs e)
    {
        SetFilterVisibility(!FilterPanel.IsVisible);
    }

    private void SetFilterVisibility(bool state)
    {
        FilterPanel.IsVisible = state;
        OpenFilterBtn.Text = FilterPanel.IsVisible ? "v" : ">";
    }
}