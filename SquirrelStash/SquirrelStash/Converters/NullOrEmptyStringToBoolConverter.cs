using System.Globalization;

namespace SquirrelStash.Converters
{
    public class NullOrEmptyStringToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns TRUE when string is null or empty.
        /// </summary>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value as string);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
