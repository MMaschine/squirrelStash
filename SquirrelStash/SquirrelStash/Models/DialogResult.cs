

namespace SquirrelStash.Models
{
    public sealed class DialogResult<T> where T : class
    {
        public bool IsCanceled { get; }
        public bool IsFailed { get; }
        public bool IsSuccess => !IsCanceled && !IsFailed;

        public T? Data { get; }

        public string? ErrorMessage { get; }

        private DialogResult(bool isCanceled, bool isFailed, T? data, string? errorMessage)
        {
            IsCanceled = isCanceled;
            IsFailed = isFailed;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static DialogResult<T> GetCanceled()
            => new(true, false, null, null);

        public static DialogResult<T> GetSuccess(T data)
            => new(false, false, data, null);

        public static DialogResult<T> GetFailed(string message)
            => new(false, true, null, message);
    }
}
