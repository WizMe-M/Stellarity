namespace Stellarity.Database.XmlConfiguration;

public class DatabaseConfiguration
{
    private readonly string _host, _port, _database, _userId, _password;

    public DatabaseConfiguration(string host, string port, string database, string userId, string password)
    {
        _host = host;
        _port = port;
        _database = database;
        _userId = userId;
        _password = password;
    }

    public static DatabaseConfiguration FromDefault()
    {
        return new DatabaseConfiguration("localhost", "5432", "Stellarity", "postgres", "password");
    }

    /// <summary>
    /// Converts <see cref="DatabaseConfiguration"/> into connection string
    /// </summary>
    /// <returns>Connection string</returns>
    public override string ToString()
    {
        return $"Server={_host}; Port={_port}; Database={_database}; User ID={_userId}; Password={_password}";
    }
}