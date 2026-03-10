using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SquirrelStash.ViewModel
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = "T-shirt";

        [ObservableProperty]
        private bool isFilterPanelOpen;

        [ObservableProperty]
        private string? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        public ObservableCollection<ItemCardViewModel> Items { get; } =
        [
            new ItemCardViewModel(),
            new ItemCardViewModel(),
            new ItemCardViewModel(),
        ];
    }
}
