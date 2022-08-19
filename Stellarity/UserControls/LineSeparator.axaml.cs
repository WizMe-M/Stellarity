using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Stellarity.UserControls;

public class LineSeparator : UserControl
{
    public static readonly StyledProperty<IBrush> ColorProperty =
        AvaloniaProperty.Register<LineSeparator, IBrush>(nameof(Color), Brushes.Black);

    public static readonly StyledProperty<double> ThicknessProperty =
        AvaloniaProperty.Register<LineSeparator, double>(nameof(Thickness), 1);

    public IBrush Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public double Thickness
    {
        get => GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }
}