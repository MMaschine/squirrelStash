using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using Microsoft.Extensions.Logging;
using SquirrelStash.Abstractions;
using SquirrelStash.Helpers;
using SquirrelStash.Logic.Factories;
using SquirrelStash.Models;
using SquirrelStash.Resources;
using System.Collections.ObjectModel;

namespace SquirrelStash.ViewModels;

public partial class OverviewPageViewModel(
    IOverviewService overviewService,
    IOverviewThresholdItemViewModelFactory thresholdItemViewModelFactory,
    ILogger<OverviewPageViewModel> logger) : ObservableObject
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
        logger.LogInformation("Loading overview.");

        var loadResult = await overviewService.GetOverviewAsync();

        if (loadResult.IsFailed)
        {
            logger.LogError("Loading overview failed. Errors: {Errors}",
                string.Join("; ", loadResult.Errors.Select(x => x.Message)));
            await MessageHelper.ShowErrorAsync(AppText.FailedToBuildOverview);
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

            logger.LogInformation(
                "Overview loaded. Categories: {CategoryCount}, Items: {ItemCount}, WarningItems: {WarningCount}, CriticalItems: {CriticalCount}.",
                TotalCategoriesCount,
                TotalItemsCount,
                WarningThresholdsReachedCount,
                CriticalThresholdsReachedCount);
        }

        HasReachedThresholds = ThresholdCategories.Any();

        IsLoading = false;
    }

    private OverviewCategoryNodeViewModel[] GetThresholdNodes(ICollection<OverviewItem> items)
    {
        var groupedItems = items.GroupBy(x => x.Category,
            (key, g) => new { Category = key, Items = g.ToList() });

        return groupedItems
            .Select(x => new OverviewCategoryNodeViewModel(x.Category, x.Items, thresholdItemViewModelFactory))
            .ToArray();
    }

    private static string FormatVersion(string version) =>
        AppText.FormatVersion(version);

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
