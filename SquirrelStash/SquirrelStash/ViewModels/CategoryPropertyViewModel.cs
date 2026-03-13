using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.Enums;

namespace SquirrelStash.ViewModels
{
    /// <summary>
    /// VM to represent the component to create property of a category 
    /// </summary>
    public partial class CategoryPropertyViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private PropertyTypes selectedType = PropertyTypes.Basic;

        public IReadOnlyList<PropertyTypes> AvailableTypes { get; } =
            Enum.GetValues<PropertyTypes>();

        public IRelayCommand<CategoryPropertyViewModel>? DeleteCommand { get; set; }
    }
}
