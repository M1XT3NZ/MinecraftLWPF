using System;
using System.Globalization;
using System.Windows.Data;

namespace MinecraftLWPF.Helpers;

public class MemoryValueToDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int memoryInMb)
        {
            var memoryInGb = memoryInMb / 1024;
            return memoryInGb + " GB";
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //throw new NotImplementedException();
        return null;
    }
}