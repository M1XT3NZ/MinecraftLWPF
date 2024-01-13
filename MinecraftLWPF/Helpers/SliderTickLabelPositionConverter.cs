using System;
using System.Globalization;
using System.Windows.Data;

namespace MinecraftLWPF.Helpers;

public class SliderTickLabelPositionMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2 || !(values[0] is double) || !(values[1] is int))
            return 0;

        var sliderWidth = (double)values[0];
        var memoryValue = (int)values[1];
        var maxMemory = (double)values[2]; // This should be passed as third value to the converter.

        // Calculate position as a percentage of the slider width
        var position = (sliderWidth - 20) * memoryValue / maxMemory; // -20 to adjust for the slider's handle width
        return position;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        //throw new NotImplementedException();
        return null;
    }
}