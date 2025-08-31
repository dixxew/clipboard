using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace clipboard.Converters;

public class IsPointerOverToVisibilityConverter : IValueConverter
{
    public object Convert(object? v, Type t, object? p, CultureInfo c)
        => v is true ? "opened" : "closed"; // классы для #Actions
    public object ConvertBack(object? v, Type t, object? p, CultureInfo c)
        => throw new NotSupportedException();
}
