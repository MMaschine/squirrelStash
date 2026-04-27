using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;

namespace SquirrelStash.ViewModels
{
    /// <summary>
    /// VM to represent the component to create property of a category 
    /// </summary>
    public partial class CategoryPropertyViewModel : ObservableObject
    {
        public CategoryPropertyViewModel(IRelayCommand<CategoryPropertyViewModel> deleteCommand)
        {
            DeleteCommand = deleteCommand;
        }

        public CategoryPropertyViewModel(
            PropertyDefinition propertyDefinition,
            IRelayCommand<CategoryPropertyViewModel> deleteCommand)
        {
            Name = propertyDefinition.Name;
            SelectedType = (PropertyTypes)propertyDefinition.TypeCode;
            Id = propertyDefinition.Id;
            AllowedValues = propertyDefinition.AllowedValues ?? string.Empty;
            DeleteCommand = deleteCommand;
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string allowedValues = string.Empty;

        [ObservableProperty]
        private PropertyTypes selectedType = PropertyTypes.Basic;

        public int? Id { get; set; }

        public bool IsNew => Id == null;

        public IReadOnlyList<PropertyTypes> AvailableTypes { get; } =
            Enum.GetValues<PropertyTypes>();

        public bool IsAllowedValuesType => SelectedType == PropertyTypes.AllowedValues;

        public IRelayCommand<CategoryPropertyViewModel>? DeleteCommand { get; set; }

        partial void OnSelectedTypeChanged(PropertyTypes value)
        {
            OnPropertyChanged(nameof(IsAllowedValuesType));
        }
    }
}
