using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public partial class Account
{
    public Account()
    {
        CommentAuthors = new HashSet<Comment>();
        CommentProfiles = new HashSet<Comment>();
        Library = new HashSet<Library>();
    }

    public Account(string email, string password) : this()
    {
        Email = email;
        Password = password;
        Balance = 0;
        RoleId = 1;
    }

    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Nickname { get; set; }
    public string Password { get; set; } = null!;
    public string? About { get; set; }

    /// <summary>
    /// Sets by default on 0
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Sets by default on current datetime
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Sets by default on false
    /// </summary>
    public bool Deleted { get; set; }

    public int RoleId { get; set; }

    public Guid? AvatarGuid { get; set; }

    public virtual Image? Avatar { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<Comment> CommentAuthors { get; set; }
    public virtual ICollection<Comment> CommentProfiles { get; set; }
    public virtual ICollection<Library> Library { get; set; }

    public static Account? Find(string email, string password)
    {
        using var context = new StellarisContext();
        return context.Users
            .Include(user => user.Role)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email && user.Password == password);
    }

    public static bool Find(string email)
    {
        using var context = new StellarisContext();
        var user = context.Users.FirstOrDefault(u => u.Email == email);
        return user != null;
    }

    public static Account Register(string email, string password)
    {
        using var context = new StellarisContext();
        var user = new Account(email, password)
        {
            RoleId = 1
        };
        context.Users.Add(user);
        context.SaveChanges();
        context.Entry(user).Reference(u => u.Role).Load();
        return user;
    }

    public void SaveChanges()
    {
        using var context = new StellarisContext();
        var user = context.Users.First(u => u.Id == Id);
        user.Nickname = Nickname;
        user.Password = Password;
        user.About = About;
        user.Balance = Balance;
        user.Deleted = Deleted;
        user.RoleId = RoleId;
        context.Users.Update(user);
        context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await using var context = new StellarisContext();
        var user = await context.Users.FirstAsync(u => u.Id == Id);
        user.Nickname = Nickname;
        user.Password = Password;
        user.About = About;
        user.Balance = Balance;
        user.Deleted = Deleted;
        user.RoleId = RoleId;
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public void Deposit(decimal depositSum)
    {
        using var context = new StellarisContext();
        var user = context.Users.First(u => u.Id == Id);
        user.Balance += depositSum;
        context.Users.Update(user);
        context.SaveChanges();
    }

#if DEBUG
    public static Account GetFirst()
    {
        using var context = new StellarisContext();
        return context.Users.First();
    }
#endif
}