using System;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Stellarity.Desktop.Image;

public static class ImagePlaceholder
{
    private const string PlaceholderUri = "avares://Stellarity.Desktop/Assets/Images/placeholder.png";

    public static Bitmap GetBitmap()
    {
        var placeholderStream = GetAsset();
        return new Bitmap(placeholderStream);
    }

    private static Stream GetAsset()
    {
        var uri = new Uri(PlaceholderUri);
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
        return assets.Open(uri);
    }
}