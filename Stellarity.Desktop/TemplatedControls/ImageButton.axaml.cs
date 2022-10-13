using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Stellarity.Desktop.TemplatedControls;

public class ImageButton : Button
{
    public ImageButton()
    {
        PropertyChanged += OnImageSet;
    }

    private void OnImageSet(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != ImageSourceProperty) return;
        var imageSet = e.NewValue is { };
        IsImageSet = imageSet;
    }

    public static readonly StyledProperty<IBitmap> ImageSourceProperty =
        AvaloniaProperty.Register<ImageButton, IBitmap>(nameof(ImageSource));

    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<ImageButton, Stretch>(nameof(Stretch), Stretch.UniformToFill);

    public static readonly StyledProperty<bool> IsImageSetProperty =
        AvaloniaProperty.Register<ImageButton, bool>(nameof(IsImageSet));

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

    public bool IsImageSet
    {
        get => GetValue(IsImageSetProperty);
        set => SetValue(IsImageSetProperty, value);
    }
}