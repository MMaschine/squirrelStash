using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.Abstractions;
using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel(IItemCardViewModelFactory itemFactory) : ObservableObject
    {
        [ObservableProperty]
        private string title = "T-shirt";

        [ObservableProperty]
        private bool isFilterPanelOpen;

        [ObservableProperty]
        private string? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        public ObservableCollection<ItemCardViewModel> Items { get; } = [];
    }
}
