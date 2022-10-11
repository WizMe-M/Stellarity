using System.Security.Cryptography;

namespace Stellarity.Domain.Cryptography;

public static class MD5Hasher
{
    public static string Encrypt(string toEncrypt)
    {
        using var md5 = MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(toEncrypt);
        var hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }
}