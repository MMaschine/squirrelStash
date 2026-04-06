using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using SquirrelStash.Abstractions;
using SquirrelStash.Helpers;
using SquirrelStash.Models;
using System.Collections.ObjectModel;

namespace SquirrelStash.ViewModels;

public partial class OverviewPageViewModel(IOverviewService overviewService) : ObservableObject
{
    public string VersionText { get; } = FormatVersion(AppInfo.Current.VersionString);

    public ObservableCollection<OverviewCategoryNodeViewModel> ThresholdCategories { get; } = [];

    public Color WarningThresholdCardColor => WarningThresholdsReachedCount == 0
        ? GetColorResource("Color.SuccessSoft")
        : GetColorResource("Color.WarningSoft");

    public Color WarningThresholdTextColor => WarningThresholdsReachedCount == 0
        ? GetColorResource("Color.SuccessGreen")
        : GetColorResource("Color.WarningOrange");

    public Color CriticalThresholdCardColor => CriticalThresholdsReachedCount == 0
        ? GetColorResource("Color.SuccessSoft")
        : GetColorResource("Color.CriticalSoft");

    public Color CriticalThresholdTextColor => CriticalThresholdsReachedCount == 0
        ? GetColorResource("Color.SuccessGreen")
        : GetColorResource("Color.CriticalRed");

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private int totalCategoriesCount;

    [ObservableProperty]
    private int totalItemsCount;

    [ObservableProperty]
    private int warningThresholdsReachedCount;

    [ObservableProperty]
    private int criticalThresholdsReachedCount;

    private bool _hasReachedThresholds;

    public bool HasReachedThresholds
    {
        get => _hasReachedThresholds;
        set
        {
            SetProperty(ref _hasReachedThresholds, value);
            ShowLogo = !value;
        }
    }

    [ObservableProperty]
    private bool showLogo = true;

    public async Task LoadOverviewAsync()
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;

        var loadResult = await overviewService.GetOverviewAsync();

        if (loadResult.IsFailed)
        {
            await MessageHelper.ShowErrorAsync("Failed to build overview");
        }
        else
        {
            var overview = loadResult.Value;

            TotalCategoriesCount = overview.TotalCategoriesCount;
            TotalItemsCount = overview.TotalItemsCount;
            WarningThresholdsReachedCount = overview.WarningThresholdsReachedCount;
            CriticalThresholdsReachedCount = overview.CriticalThresholdsReachedCount;

            ThresholdCategories.Clear();

            foreach (var item in GetThresholdNodes(loadResult.Value.ItemsToHighlight))
            {
                ThresholdCategories.Add(item);
            }
        }

        HasReachedThresholds = ThresholdCategories.Any(); 

        IsLoading = false;
    }

    private OverviewCategoryNodeViewModel[] GetThresholdNodes(ICollection<OverviewItem> items)
    {
        var groupedItems = items.GroupBy(x => x.Category, 
            (key, g) => new { Category = key, Items = g.ToList() });

       return groupedItems.Select(x => new OverviewCategoryNodeViewModel(x.Category, x.Items)).ToArray();

    }

    private static string FormatVersion(string version) =>
        version.Equals("0.1-alpha.1", StringComparison.OrdinalIgnoreCase)
            ? "Version 0.1 Alpha 1"
            : $"Version {version}";

    partial void OnWarningThresholdsReachedCountChanged(int value)
    {
        OnPropertyChanged(nameof(WarningThresholdCardColor));
        OnPropertyChanged(nameof(WarningThresholdTextColor));
    }

    partial void OnCriticalThresholdsReachedCountChanged(int value)
    {
        OnPropertyChanged(nameof(CriticalThresholdCardColor));
        OnPropertyChanged(nameof(CriticalThresholdTextColor));
    }

    private static Color GetColorResource(string key) =>
        (Color)Application.Current!.Resources[key];
}
