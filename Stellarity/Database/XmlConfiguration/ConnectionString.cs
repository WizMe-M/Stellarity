namespace Stellarity.Database.XmlConfiguration;

public record ConnectionString(string Host, string Port, string Database, string UserId, string Password)
{
    public static ConnectionString FromDefault() =>
        new("localhost", "5432", "Stellarity", "postgres", "password");

    /// <summary>
    /// Converts <see cref="ConnectionString"/> into connection string
    /// </summary>
    /// <returns>Connection string</returns>
    public override string ToString() =>
        $"Server={Host}; Port={Port}; Database={Database}; User ID={UserId}; Password={Password}";
}