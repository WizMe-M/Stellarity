using System.Drawing;
using System.IO;

namespace Stellarity.Extensions;

public static class ByteExtensions
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
}