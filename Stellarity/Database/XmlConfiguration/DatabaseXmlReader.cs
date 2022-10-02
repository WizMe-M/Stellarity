using System.Xml;

namespace Stellarity.Database.XmlConfiguration;

public static class DatabaseXmlReader
{
    public static DatabaseConfiguration ParseXmlFile(string path)
    {
        var config = new DatabaseConfiguration();
        
        var xml = new XmlDocument();
        xml.Load(path);
        var root = xml.DocumentElement!;
        foreach (XmlElement node in root)
        {
            switch (node.Name)
            {
                case "server":
                    config.Host = node.GetAttribute("host");
                    config.Port = node.GetAttribute("port");
                    break;
                case "database":
                    config.Database = node.GetAttribute("name");
                    break;
                case "user_access":
                    config.UserId = node.GetAttribute("user_id");
                    config.Password = node.GetAttribute("password");
                    break;
            }
        }

        return config;
    }
    
    public class DatabaseConfiguration
    {
        public string Host { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;

        /// <summary>
        /// Converts <see cref="DatabaseConfiguration"/> into connection string
        /// </summary>
        /// <returns>Connection string</returns>
        public override string ToString()
        {
            return $"Server={Host}; Port={Port}; Database={Database}; User ID={UserId}; Password={Password}";
        }
    }
}