using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Stellarity.UserControls;

public class ImageButton : Button
{
    public static readonly StyledProperty<IBitmap> ImageSourceProperty = AvaloniaProperty.Register<ImageButton, IBitmap>(
        nameof(ImageSource));

    public static readonly StyledProperty<Stretch> StretchProperty = AvaloniaProperty.Register<ImageButton, Stretch>(
        nameof(Stretch), Stretch.UniformToFill);
    
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
        Border.BoxShadowProperty.AddOwner<ImageButton>();
    public IBitmap ImageSource
    {
        get => GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }
    
    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}