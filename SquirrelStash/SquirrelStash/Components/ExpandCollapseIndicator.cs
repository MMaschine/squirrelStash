namespace SquirrelStash.Components;

public class ExpandCollapseIndicator : ContentView
{
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(ExpandCollapseIndicator),
        false,
        propertyChanged: OnIsExpandedChanged);

    private readonly Label _label;

    public ExpandCollapseIndicator()
    {
        InputTransparent = true;
        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Center;

        _label = new Label
        {
            Text = "v",
            FontFamily = "OpenSansSemibold",
            FontSize = 14,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            WidthRequest = 18,
            MinimumWidthRequest = 18
        };
        _label.SetDynamicResource(Label.TextColorProperty, "Color.PrimaryBlue");

        Content = _label;
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
        _label.Rotation = IsExpanded ? 180 : 0;
    }
}
