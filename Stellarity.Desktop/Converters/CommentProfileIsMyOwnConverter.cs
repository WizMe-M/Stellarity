using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.Converters;

public class CommentProfileIsMyOwnConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not { Count: 2 } || values[0] is not Account profile || values[1] is not Account viewer)
            return false;
        return viewer.IsIdenticalWith(profile);
    }
}