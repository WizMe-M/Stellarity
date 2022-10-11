namespace Stellarity.Domain.Cryptography;

public sealed class HashedPassword
{
    private HashedPassword(string encryptedPassword) => Password = encryptedPassword;

    public string Password { get; }

    public static HashedPassword FromEncrypted(string encryptedPassword) => new HashedPassword(encryptedPassword);

    public static HashedPassword FromDecrypted(string passwordString)
    {
        var hashed = MD5Hasher.Encrypt(passwordString);
        return new HashedPassword(hashed);
    }
}