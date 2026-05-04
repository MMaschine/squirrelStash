using Microsoft.Extensions.Logging;
using SquirrelStash.Resources;
using SquirrelStash.Views;

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

        public static async Task<bool> ShowConfirmationAsync(string message, string title = "")
        {
            var dialog = new ConfirmationDialog(message, title);

            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!.Navigation.PushModalAsync(dialog));

            return await dialog.ResultTask;
        }

        public static async Task NotifyException(Exception exception, string message, ILogger logger)
        {
            try
            {
                // log first
                logger.LogError(exception, "Exception");

                var shouldShare = await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current!.MainPage!.DisplayAlert(
                    AppText.AlertErrorTitle,
                    AppText.SomethingWentWrongShareLogs,
                    AppText.AlertSend,
                    AppText.AlertCancel));

                if (shouldShare)
                {
                    var files = Directory.GetFiles(FileSystem.AppDataDirectory);
                    var logPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, $"log{DateTime.Today.Date:yyyyMMdd}.txt");

                    if (!string.IsNullOrWhiteSpace(logPath) && File.Exists(logPath))
                    {
                        await Share.Default.RequestAsync(new ShareFileRequest
                        {
                            Title = AppText.SendLogFileTitle,
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
