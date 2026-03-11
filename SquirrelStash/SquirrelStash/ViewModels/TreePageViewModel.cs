using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using SquirrelStash.Abstractions;


namespace SquirrelStash.ViewModels
{
    public partial class TreePageViewModel(IItemCardViewModelFactory factory) : ObservableObject
    {
        public ObservableCollection<CategoryCardViewModel> Categories { get; } =
        [
            new CategoryCardViewModel(factory)
            {
                Title = "Test category 1"
            },
            new CategoryCardViewModel(factory)
            {
                Title = "Test category 2"
            }
        ];
    }
}
