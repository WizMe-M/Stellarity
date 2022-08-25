using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Stellarity.Database.Entities;

namespace Stellarity.Converters;

public class CommentAlignmentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = HorizontalAlignment.Left;
        if (value is not Comment comment) return result;
        result = comment.Author == comment.Profile ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        return result;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}