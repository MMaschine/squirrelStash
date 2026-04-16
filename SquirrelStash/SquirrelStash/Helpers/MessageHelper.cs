using Microsoft.Extensions.Logging;
using SquirrelStash.Resources;

namespace SquirrelStash.Helpers
{
    public static class MessageHelper
    {
        public static async Task ShowErrorAsync(string message, string title = "")
        {
            await ShowAlertAsync(
                string.IsNullOrWhiteSpace(title) ? AppText.AlertErrorTitle : title,
                message,
                AppText.AlertOk);
        }

        public static async Task ShowInfoAsync(string message, string title = "")
        {
            await ShowAlertAsync(
                string.IsNullOrWhiteSpace(title) ? AppText.AlertInformationTitle : title,
                message,
                AppText.AlertOk);
        }

        public static async Task ShowWarningAsync(string message, string title = "")
        {
            await ShowAlertAsync(
                string.IsNullOrWhiteSpace(title) ? AppText.AlertWarningTitle : title,
                message,
                AppText.AlertOk);
        }

        public static async Task NotifyException(Exception exception, string message, ILogger logger)
        {
            try
            {
                // log first
                logger.LogError(exception, "Exception");

                var shouldShare = await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current!.MainPage!.DisplayAlert(
                    "Error",
                    "Something went wrong.\nWould you like to send logs?",
                    "Send",
                    "Cancel"));

                if (shouldShare)
                {
                    var files = Directory.GetFiles(FileSystem.AppDataDirectory);
                    var logPath = Path.Combine(FileSystem.AppDataDirectory, $"log{DateTime.Today.Date:yyyyMMdd}.txt");

                    if (!string.IsNullOrWhiteSpace(logPath) && File.Exists(logPath))
                    {
                        await Share.Default.RequestAsync(new ShareFileRequest
                        {
                            Title = "Send log file",
                            File = new ShareFile(logPath)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to share the log file. Message: {ex.Message}");
            }
        }

        private static async Task ShowAlertAsync(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!
                    .DisplayAlert(title, message, cancel));
        }
    }
}
