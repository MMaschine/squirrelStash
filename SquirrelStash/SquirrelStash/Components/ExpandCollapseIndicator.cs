namespace SquirrelStash.Components;

public partial class ExpandCollapseIndicator : ContentView
{
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(ExpandCollapseIndicator),
        false,
        propertyChanged: OnIsExpandedChanged);

    public ExpandCollapseIndicator()
    {
        InitializeComponent();
        UpdateIndicator();
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((ExpandCollapseIndicator)bindable).UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        RotationBox.TranslationX = 0;
        RotationBox.TranslationY = 0;
        RotationBox.Rotation = IsExpanded ? 180 : 0;
    }
}
