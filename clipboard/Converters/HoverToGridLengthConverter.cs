using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace clipboard.Converters;

public class HoverToGridLengthConverter : IValueConverter
{
    public double Collapsed { get; set; } = 0;
    public double Expanded  { get; set; } = 48; // под 28px кнопки + отступы

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new GridLength(value is true ? Expanded : Collapsed);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
