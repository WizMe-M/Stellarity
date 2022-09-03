using System.Security.Cryptography;
using System.Text;

namespace Stellarity.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Hashes string
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>MD5-hashed string (a sequence of `2 upper case hexadecimal` symbols)</returns>
    public static string CreateMD5(this string input)
    {
        // Use input string to calculate MD5 hash
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        var sb = new StringBuilder();
        // X2 means two uppercase hexadecimal symbols for every character
        foreach (var b in hashBytes) sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}