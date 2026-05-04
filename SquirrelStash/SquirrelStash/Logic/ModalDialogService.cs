using SquirrelStash.Abstractions;

namespace SquirrelStash.Logic;

internal class ModalDialogService : IModalDialogService
{
    /// <inheritdoc />
    public async Task<TResult> ShowAsync<TResult>(IModalDialog<TResult> dialog)
    {
        try
        {
            await Shell.Current.CurrentPage.Navigation.PushModalAsync((ContentPage)dialog);

            return await dialog.DialogResultTask;
        }
        finally
        {
            dialog.Dispose();
        }
    }
}
