using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Avalonia.Media.Imaging;

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

    public static byte[] ToBytes<T>(this T obj)
    {
        if (obj is null) throw new InvalidOperationException("Convertable object was null");

        var bf = new BinaryFormatter();
        using var ms = new MemoryStream();
#pragma warning disable SYSLIB0011
        bf.Serialize(ms, obj);
#pragma warning restore SYSLIB0011
        return ms.ToArray();
    }

    public static T FromBytes<T>(this byte[] buffer)
    {
        if (buffer is null) throw new InvalidOperationException("Bytes was null");

        using var ms = new MemoryStream(buffer);
        var bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
        return (T)bf.Deserialize(ms);
#pragma warning restore SYSLIB0011
    }
}