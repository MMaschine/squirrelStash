using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.Models;

namespace SquirrelStash.ViewModels;

public partial class OverviewCategoryNodeViewModel : ObservableObject
{
    public OverviewCategoryNodeViewModel(string title, ICollection<OverviewItem> overviewItems)
    {
        Title = title;

        Items = overviewItems
            .Select(item => new OverviewThresholdItemViewModel(item)).ToList();
    }

    public IReadOnlyList<OverviewThresholdItemViewModel> Items { get; }

    [ObservableProperty]
    private string title;
}