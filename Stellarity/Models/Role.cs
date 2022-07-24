using System.Collections.Generic;

namespace Stellarity.Models;

public partial class Role
{
    public Role()
    {
        Users = new HashSet<User>();
    }

    public Role(string name) : this()
    {
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; }

    public bool CanAddGames => Id == 1;
}