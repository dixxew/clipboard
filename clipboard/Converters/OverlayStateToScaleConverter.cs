using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace clipboard.Converters;

public class OverlayStateToScaleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? 1.0 : 0.0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => (double)value! == 1.0 ? true : false;
}