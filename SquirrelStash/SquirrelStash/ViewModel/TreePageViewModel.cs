using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace SquirrelStash.ViewModel
{
    public partial class TreePageViewModel : ObservableObject
    {
        public ObservableCollection<CategoryCardViewModel> Categories { get; } =
        [
            new CategoryCardViewModel()
            {
                Title = "Test category 1"
            },
            new CategoryCardViewModel()
            {
                Title = "Test category 2"
            }
        ];
    }
}
