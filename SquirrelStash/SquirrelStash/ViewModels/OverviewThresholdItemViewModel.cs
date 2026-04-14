using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SquirrelStash.Models;

namespace SquirrelStash.ViewModels;

public partial class OverviewThresholdItemViewModel : ObservableObject
{
    public OverviewThresholdItemViewModel(OverviewItem item, ILogger<OverviewThresholdItemViewModel> logger)
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
            logger.LogError(e, "Failed to resolve threshold marker color for item {ItemName}.", item.Name);
        }
    }

    [ObservableProperty]
    private string displayText;

    [ObservableProperty]
    private bool isCritical;

    [ObservableProperty]
    private Color markerColor;
}
