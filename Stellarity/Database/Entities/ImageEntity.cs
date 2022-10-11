namespace Stellarity.Database.Entities;

public partial class ImageEntity : IEntity
{
    public ImageEntity()
    {
        // Guid = Guid.NewGuid();
        Games = new HashSet<GameEntity>();
        Users = new HashSet<AccountEntity>();
    }

    public ImageEntity(Guid guid, string name, byte[] data) : this()
    {
        Guid = guid;
        Data = data;
        Name = name;
    }

    public ImageEntity(string name, byte[] data) : this()
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

    public virtual ICollection<GameEntity> Games { get; set; }
    public virtual ICollection<AccountEntity> Users { get; set; }

    public static async Task<ImageEntity> AddFromAsync(GameEntity game, byte[] coverData)
    {
        await using var context = new StellarityContext();
        var img = new ImageEntity(game.Title, coverData);
        await context.Images.AddAsync(img);
        await context.SaveChangesAsync();
        return context.Entry(img).Entity;
    }

    public static ImageEntity? Find(Guid? guid)
    {
        using var context = new StellarityContext();
        return context.Images.FirstOrDefault(img => img.Guid == guid);
    }
}