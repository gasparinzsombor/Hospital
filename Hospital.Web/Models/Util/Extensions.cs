using System.Globalization;

namespace Hospital.Models.Util;

public static class Extensions
{
    public static string ToUrlCompatibleString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd-HH");
    }

    public static DateTime ToUrlDateTime(this string str)
    {
        return DateTime.ParseExact(str, "yyyy-MM-dd-HH", CultureInfo.InvariantCulture);
    }

    public static string ToInputString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }
}