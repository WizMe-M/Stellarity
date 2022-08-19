using System.Collections.Generic;

namespace Stellarity.Database.Entities;

public partial class Role
{
    public Role()
    {
        Users = new HashSet<Account>();
    }

    public Role(string name) : this()
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Users { get; set; }

    public bool CanAddGames => Id == 1;
}