namespace SquirrelStash.Abstractions;

/// <summary>
/// Represents a modal dialog page that produces a typed result and owns disposable resources.
/// </summary>
/// <typeparam name="TResult">The result type produced by the dialog.</typeparam>
public interface IModalDialog<TResult> : IDisposable
{
    /// <summary>
    /// Gets the task completed when the dialog produces its result.
    /// </summary>
    Task<TResult> DialogResultTask { get; }
}
