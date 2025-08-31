using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace clipboard.Converters;

public class WindowActiveToBorderBrushConverter : AvaloniaObject, IValueConverter
{
    public static readonly StyledProperty<IBrush> ActiveBrushProperty =
        AvaloniaProperty.Register<WindowActiveToBorderBrushConverter, IBrush>(nameof(ActiveBrush));

    public static readonly StyledProperty<IBrush> InactiveBrushProperty =
        AvaloniaProperty.Register<WindowActiveToBorderBrushConverter, IBrush>(nameof(InactiveBrush));

    public IBrush ActiveBrush
    {
        get => GetValue(ActiveBrushProperty);
        set => SetValue(ActiveBrushProperty, value);
    }

    public IBrush InactiveBrush
    {
        get => GetValue(InactiveBrushProperty);
        set => SetValue(InactiveBrushProperty, value);
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? ActiveBrush! : InactiveBrush!;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}