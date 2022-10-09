namespace Stellarity.Database.Entities;

public partial class Image
{
    public Image()
    {
        // Guid = Guid.NewGuid();
        Games = new HashSet<Game>();
        Users = new HashSet<Account>();
    }

    public Image(Guid guid, string name, byte[] data) : this()
    {
        Guid = guid;
        Data = data;
        Name = name;
    }

    public Image(string name, byte[] data) : this()
    {
        Name = name;
        Data = data;
    }

    /// <summary>
    /// Содержит уникальный идентификатор изображения. Может использоваться в качестве имени файла для кэша
    /// </summary>
    public Guid Guid { get; set; }

    public byte[] Data { get; set; } = null!;
    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; }
    public virtual ICollection<Account> Users { get; set; }

    public static async Task<Image> AddFromAsync(Game game, byte[] coverData)
    {
        await using var context = new StellarityContext(); 
        var img = new Image(game.Title, coverData);
        await context.Images.AddAsync(img);
        await context.SaveChangesAsync();
        return context.Entry(img).Entity;
    }

    public static Image? Find(Guid? guid)
    {
        using var context = new StellarityContext();
        return context.Images.FirstOrDefault(img => img.Guid == guid);
    }
}