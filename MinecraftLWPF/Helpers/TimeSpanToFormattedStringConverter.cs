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
            // Check if the TimeSpan includes days
            if (timeSpan.Days > 0)
                // Format to include days, hours, and minutes
                return
                    $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}, {timeSpan.Hours} hour{(timeSpan.Hours != 1 ? "s" : "")}, {timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
            if (timeSpan.TotalHours >= 1)
                // Format to include hours and minutes
                return
                    $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours >= 2 ? "s" : "")}, {timeSpan.Minutes} minute{(timeSpan.Minutes != 1 ? "s" : "")}";
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