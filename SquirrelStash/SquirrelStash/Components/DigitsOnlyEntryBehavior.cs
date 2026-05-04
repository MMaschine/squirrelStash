namespace SquirrelStash.Components;

/// <summary>
/// Filters an entry value so it contains only digit characters.
/// </summary>
public sealed class DigitsOnlyEntryBehavior : Behavior<Entry>
{
    /// <inheritdoc />
    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.TextChanged += OnTextChanged;
    }

    /// <inheritdoc />
    protected override void OnDetachingFrom(Entry bindable)
    {
        bindable.TextChanged -= OnTextChanged;
        base.OnDetachingFrom(bindable);
    }

    private static void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
        {
            return;
        }

        var text = e.NewTextValue ?? string.Empty;
        var digitsOnly = new string(text.Where(char.IsDigit).ToArray());

        if (text != digitsOnly)
        {
            entry.Text = digitsOnly;
        }
    }
}
