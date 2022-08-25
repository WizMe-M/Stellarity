using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Stellarity.Database.Entities;

namespace Stellarity.Converters;

public class CommentCornerRadiusConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Comment comment) return new CornerRadius();
        var result = comment.Author == comment.Profile
            ? new CornerRadius(10, 10, 0, 10)
            : new CornerRadius(10, 10, 10, 0);
        return result;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}