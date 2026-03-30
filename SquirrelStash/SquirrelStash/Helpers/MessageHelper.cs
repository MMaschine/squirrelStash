namespace SquirrelStash.Helpers
{
    public static class MessageHelper 
    {
        public static async Task ShowErrorAsync(string message, string title = "")
        {
            await ShowAlertAsync(title ?? "Error", message, "OK");
        }

        public static async Task ShowInfoAsync(string message, string title = "")
        {
            await ShowAlertAsync(title ?? "Information", message, "OK");
        }

        public static async Task ShowWarningAsync(string message, string title = "")
        {
            await ShowAlertAsync(title ?? "Warning!", message, "OK");
        }

        private static async Task ShowAlertAsync(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!
                    .DisplayAlert(title, message, cancel));
        }
    }
}
