using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.Models;

namespace SquirrelStash.ViewModels;

public partial class OverviewThresholdItemViewModel : ObservableObject
{
    public OverviewThresholdItemViewModel(OverviewItem item)
    {
        DisplayText = $"{item.Name} : {item.Quantity}";
        IsCritical = item.IsCritical;

        try
        {
            MarkerColor = item.IsCritical
                ? (Color)Application.Current!.Resources["Color.CriticalRed"]
                : (Color)Application.Current!.Resources["Color.WarningOrange"];
        }
        catch (Exception e)
        {
            var s = e.Message;
        }
    }

    [ObservableProperty]
    private string displayText;

    [ObservableProperty]
    private bool isCritical;

    [ObservableProperty]
    private Color markerColor;
}