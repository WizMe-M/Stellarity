using System.Runtime.Serialization.Formatters.Binary;

namespace Stellarity.Domain.Extensions;

public static class ByteExtensions
{
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