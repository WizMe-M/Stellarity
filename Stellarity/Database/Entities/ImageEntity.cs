namespace Stellarity.Database.Entities;

public partial class ImageEntity : IEntity
{
    public Guid Guid { get; set; }

    public byte[] Data { get; set; } = null!;
    public string Name { get; set; } = null!;

    public virtual ICollection<GameEntity>? Games { get; set; }
    public virtual ICollection<AccountEntity>? Users { get; set; }
}