using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Stellarity.Desktop.TemplatedControls;

public class GameCard : Button
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
        AvaloniaProperty.Register<GameCard, BoxShadows>(nameof(BoxShadow));

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<GameCard, string>(
        nameof(Title));

    public static readonly StyledProperty<string> CostStringProperty = AvaloniaProperty.Register<GameCard, string>(
        nameof(CostString));

    public static readonly StyledProperty<IImage> CoverProperty = AvaloniaProperty.Register<GameCard, IImage>(
        nameof(Cover));

    public static readonly StyledProperty<string> PurchaseDateStringProperty =
        AvaloniaProperty.Register<GameCard, string>(
            nameof(PurchaseDateString));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string CostString
    {
        get => GetValue(CostStringProperty);
        set => SetValue(CostStringProperty, value);
    }

    public IImage Cover
    {
        get => GetValue(CoverProperty);
        set => SetValue(CoverProperty, value);
    }

    public string PurchaseDateString
    {
        get => GetValue(PurchaseDateStringProperty);
        set => SetValue(PurchaseDateStringProperty, value);
    }

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}