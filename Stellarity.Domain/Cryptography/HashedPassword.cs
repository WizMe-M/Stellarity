namespace Stellarity.Domain.Cryptography;

public record HashedPassword(string Password)
{
    public static HashedPassword FromEncrypted(string encryptedPassword) => new(encryptedPassword);

    public static HashedPassword FromDecrypted(string passwordString)
    {
        var hash = MD5Hasher.Encrypt(passwordString);
        return new HashedPassword(hash);
    }
}