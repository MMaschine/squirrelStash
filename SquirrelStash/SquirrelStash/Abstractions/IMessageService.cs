namespace SquirrelStash.Abstractions;

public interface IMessageService
{
    Task ShowErrorAsync(string message, string title = "");

    Task ShowInfoAsync(string message, string title = "");

    Task<bool> ShowConfirmationAsync(string message, string title = "");
}