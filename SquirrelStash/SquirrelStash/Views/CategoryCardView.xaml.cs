using SquirrelStash.ViewModels;

namespace SquirrelStash.Views;

public partial class CategoryCardView : ContentView
{
	public CategoryCardView()
	{
		InitializeComponent();
        SetFilterVisibility(false);
        SetItemLayoutVisibility(false);
    }

    private void OpenFilterOnClicked(object? sender, EventArgs e)
    {
        SetFilterVisibility(!FilterPanel.IsVisible);
    }

    private void OnItemLayoutTapped(object? sender, TappedEventArgs e)
    {
        SetItemLayoutVisibility(!ItemsLayout.IsVisible);
    }

    private void SetFilterVisibility(bool state)
    {
        FilterPanel.IsVisible = state;
        OpenFilterBtn.Text = FilterPanel.IsVisible ? "v" : ">";
    }

    private void SetItemLayoutVisibility(bool state)
    {
        ItemsLayout.IsVisible = state;
        ItemsArrow.Text = ItemsLayout.IsVisible ? "v" : ">";
    }
}