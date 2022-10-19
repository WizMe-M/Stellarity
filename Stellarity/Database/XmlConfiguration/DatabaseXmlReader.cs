using System.Xml;

namespace Stellarity.Database.XmlConfiguration;

public static class DatabaseXmlReader
{
    public static DatabaseConfiguration ParseXmlFile(string path)
    {
        var host = string.Empty;
        var port = string.Empty;
        var database = string.Empty;
        var userId = string.Empty;
        var password = string.Empty;

        var xml = new XmlDocument();
        xml.Load(path);
        var root = xml.DocumentElement!;
        foreach (XmlElement node in root)
        {
            switch (node.Name)
            {
                case "server":
                    host = node.GetAttribute(nameof(host));
                    port = node.GetAttribute(nameof(port));
                    break;
                case "database":
                    database = node.GetAttribute(nameof(database));
                    break;
                case "user_access":
                    userId = node.GetAttribute(nameof(userId));
                    password = node.GetAttribute(nameof(password));
                    break;
            }
        }

        return new DatabaseConfiguration(host, port, database, userId, password);
    }
}