namespace Stellarity.Database.Entities;

public partial class RoleEntity
{
    public RoleEntity()
    {
        Users = new HashSet<AccountEntity>();
    }

    public RoleEntity(string name) : this()
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<AccountEntity> Users { get; set; }
}