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

        private static async Task ShowAlertAsync(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!
                    .DisplayAlert(title, message, cancel));
        }
    }
}
