namespace MauiAuth0App.Converter
{
    public class UnixTimeStamp
    {
        public static DateTime? UnixTimeStampToDateTime(string unixTime)
        {
            var success = double.TryParse(unixTime, out double c);
            if (!success)
                return null;
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddMilliseconds(c).ToLocalTime();
        }

        public static string UnixTimeStampToString(string unixTime)
        {
            var dateTime = UnixTimeStampToDateTime(unixTime);
            if (dateTime == null)
                return unixTime;
            return dateTime.ToString();
        }
    }
}
