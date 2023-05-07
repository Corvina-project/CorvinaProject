namespace MauiAuth0App.Converter;

public class UnixDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null) return null;

        double timestamp;
        if (!double.TryParse(value.ToString(), out timestamp))
            throw new ArgumentException("Value must be a valid Unix timestamp");

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp).ToLocalTime();

        return dateTime;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null) return null;

        DateTime dateTime;
        if (!DateTime.TryParse(value.ToString(), out dateTime))
            throw new ArgumentException("Value must be a valid DateTime");

        double timestamp = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

        return timestamp.ToString();
    }
}