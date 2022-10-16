using System.IO;
using Avalonia.Media.Imaging;

namespace Stellarity.Desktop.Extensions;

public static class BitmapExtensions
{
    /// <summary>
    /// Converts image data to Bitmap
    /// </summary>
    /// <param name="data">Byte array of image's data</param>
    /// <returns>Bitmap or null, if data was null</returns>
    public static Bitmap? ToBitmap(this byte[]? data)
    {
        if (data is null) return null;

        using var ms = new MemoryStream(data);
        return new Bitmap(ms);
    }

    public static byte[]? FromBitmap(this Bitmap? img)
    {
        if (img is null) return null;

        using var ms = new MemoryStream();
        img.Save(ms);
        return ms.ToArray();
    }
}