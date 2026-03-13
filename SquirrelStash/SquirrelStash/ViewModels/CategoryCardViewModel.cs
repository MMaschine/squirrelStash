using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.DataAccess.Entities;


namespace SquirrelStash.ViewModels
{
    public partial class CategoryCardViewModel(Category category) : ObservableObject
    {
        [ObservableProperty]
        private string title = category.Title;

        [ObservableProperty]
        private bool isFilterPanelOpen;

        [ObservableProperty]
        private string? selectedFilter;

        [ObservableProperty]
        private string? filterValue;

        public ObservableCollection<ItemCardViewModel> Items { get; } = [];
    }
}
