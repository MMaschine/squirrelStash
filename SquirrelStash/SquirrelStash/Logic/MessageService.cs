using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquirrelStash.Abstractions;

namespace SquirrelStash.Logic
{
    public class MessageService : IMessageService
    {
        public async Task ShowErrorAsync(string message, string title = "")
        {
            await ShowAlertAsync(title ?? "Error", message, "OK");
        }

        public async Task ShowInfoAsync(string message, string title = "")
        {
            await ShowAlertAsync(title ?? "Information", message, "OK");
        }

        public async Task<bool> ShowConfirmationAsync(string message, string title = "")
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!
                    .DisplayAlert(title ?? "Confirmation", message, "Yes", "No"));
        }

        private static async Task ShowAlertAsync(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Application.Current!.MainPage!
                    .DisplayAlert(title, message, cancel));
        }
    }
}
