using System;
using System.Globalization;
using System.Windows.Data;

namespace MinecraftLWPF.Helpers;

public class TimeSpanToFormattedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            // Check if the TimeSpan includes years
            if (timeSpan.Days >= 365)
            {
                // Format to include years, days, hours, and minutes
                return $"{timeSpan.Days / 365} year{(timeSpan.Days / 365 > 1 ? "s" : "")}, {(timeSpan.Days % 365)} day{(timeSpan.Days % 365 > 1 ? "s" : "")}, {timeSpan.Hours} hour{(timeSpan.Hours != 1 ? "s" : "")}, {timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
            }
            // Check if the TimeSpan includes days
            else if (timeSpan.Days > 0)
            {
                // Format to include days, hours, and minutes
                return $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}, {timeSpan.Hours} hour{(timeSpan.Hours != 1 ? "s" : "")}, {timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                // Format to include hours and minutes
                return $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours >= 2 ? "s" : "")}, {timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
            }
            // Only minutes are present
            return $"{timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
        }

        return string.Empty;
    }



    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}