namespace SquirrelStash.Helpers;

internal static class MessageHelper
{
    public static Task ShowErrorAsync(string message, string title = "")
    {
        return Task.CompletedTask;
    }

    public static Task ShowWarningAsync(string message, string title = "")
    {
        return Task.CompletedTask;
    }

    public static Task<bool> ShowConfirmationAsync(string message, string title = "")
    {
        return Task.FromResult(false);
    }

    public static Task NotifyException(Exception exception, string message, Microsoft.Extensions.Logging.ILogger logger)
    {
        return Task.CompletedTask;
    }
}
