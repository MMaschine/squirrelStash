using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.Abstractions;
using SquirrelStash.Helpers;
using System.Collections.ObjectModel;
using SquirrelStash.Models;

namespace SquirrelStash.ViewModels;

public partial class OverviewPageViewModel(IOverviewService overviewService) : ObservableObject
{
    public ObservableCollection<OverviewCategoryNodeViewModel> ThresholdCategories { get; } = [];

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

}