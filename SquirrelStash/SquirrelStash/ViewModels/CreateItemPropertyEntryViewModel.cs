using CommunityToolkit.Mvvm.ComponentModel;
using SquirrelStash.DataAccess.Entities;
using SquirrelStash.Enums;

namespace SquirrelStash.ViewModels
{
    public partial class CreateItemPropertyEntryViewModel : ObservableObject
    {
        public CreateItemPropertyEntryViewModel(PropertyDefinition definition)
        {
            ArgumentNullException.ThrowIfNull(definition);

            DefinitionId = definition.Id;
            Name = definition.Name;
            Type = (PropertyTypes)definition.TypeCode;

            AllowedValues = ParseAllowedValues(definition.AllowedValues).ToArray();
        }

        public int DefinitionId { get; }

        public string Name { get; }

        public PropertyTypes Type { get; }

        public Keyboard ValueKeyboard =>
            Type == PropertyTypes.Numeric
                ? Keyboard.Numeric
                : Keyboard.Default;

        public IReadOnlyList<string> AllowedValues { get; }

        public bool IsAllowedValuesType => Type == PropertyTypes.AllowedValues;

        public bool IsManualValueType => !IsAllowedValuesType;

        public bool HasMissingValue => string.IsNullOrWhiteSpace(Value);

        [ObservableProperty]
        private string value = string.Empty;

        [ObservableProperty]
        private string? selectedAllowedValue;

        partial void OnSelectedAllowedValueChanged(string? value)
        {
            Value = value ?? string.Empty;
        }

        partial void OnValueChanged(string value)
        {
            OnPropertyChanged(nameof(HasMissingValue));

            if (IsAllowedValuesType &&
                !string.Equals(SelectedAllowedValue, value, StringComparison.Ordinal) &&
                AllowedValues.Contains(value))
            {
                SelectedAllowedValue = value;
            }
        }

        private static IEnumerable<string> ParseAllowedValues(string? allowedValues)
        {
            if (string.IsNullOrWhiteSpace(allowedValues))
            {
                return [];
            }

            return allowedValues
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
