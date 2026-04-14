using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.Logic.Factories;
using SquirrelStash.Models;

namespace SquirrelStash.ViewModels;

public partial class OverviewCategoryNodeViewModel : ObservableObject
{
    public OverviewCategoryNodeViewModel(
        string title,
        ICollection<OverviewItem> overviewItems,
        IOverviewThresholdItemViewModelFactory thresholdItemViewModelFactory)
    {
        Title = title;

        Items = overviewItems
            .Select(thresholdItemViewModelFactory.GetViewModel)
            .ToList();
    }

    public IReadOnlyList<OverviewThresholdItemViewModel> Items { get; }

    [ObservableProperty]
    private string title;
}
