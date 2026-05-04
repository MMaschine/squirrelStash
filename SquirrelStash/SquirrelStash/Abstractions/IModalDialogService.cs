namespace SquirrelStash.Abstractions;

/// <summary>
/// Shows modal dialog pages and returns their asynchronous result.
/// </summary>
public interface IModalDialogService
{
    /// <summary>
    /// Pushes the dialog page modally, awaits its result task, and disposes the page when supported.
    /// </summary>
    /// <typeparam name="TResult">The result type produced by the dialog.</typeparam>
    /// <param name="dialog">The dialog page to show.</param>
    /// <returns>The dialog result.</returns>
    Task<TResult> ShowAsync<TResult>(IModalDialog<TResult> dialog);
}
