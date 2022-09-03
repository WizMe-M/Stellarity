using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Stellarity.Services;

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

    // TODO: make Nickname not null
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

    public static bool Exists(string email)
    {
        using var context = new StellarisContext();
        var user = context.Users.FirstOrDefault(u => u.Email == email);
        return user is { };
    }

    public static Account? Find(string email, string password)
    {
        using var context = new StellarisContext();
        return context.Users
            .Include(user => user.Role)
            .Include(user => user.Avatar)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email && user.Password == password);
    }

    public static Account? Find(string email)
    {
        using var context = new StellarisContext();
        return context.Users
            .Include(user => user.Role)
            .Include(user => user.Avatar)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email);
    }

    public static Account Register(string email, string password, Roles role)
    {
        using var context = new StellarisContext();
        var gamer = new Account(email, password)
        {
            RoleId = (int)role
        };
        context.Users.Add(gamer);
        context.SaveChanges();
        context.Entry(gamer).Reference(u => u.Role).Load();
        return gamer;
    }

    public async Task SaveChangesAsync()
    {
        if (Nickname?.Trim().Length == 0) Nickname = null;
        if (About?.Trim().Length == 0) About = null;

        await using var context = new StellarisContext();
        var user = context.Entry(this).Entity;
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task ChangeAvatarAsync(byte[]? avatarData)
    {
        if (avatarData is null) return;

        await using var context = new StellarisContext();
        var user = context.Entry(this).Entity;
        context.Users.Attach(user);
        if (user.AvatarGuid is null)
        {
            var avatar = new Image(user.Email) { Data = avatarData };
            user.Avatar = avatar;
            context.Users.Update(user);
        }
        else
        {
            if (user.Avatar is null)
                await context.Entry(user).Reference(account => account.Avatar).LoadAsync();
            user.Avatar!.Data = avatarData;
            context.Images.Update(user.Avatar!);
        }

        await context.SaveChangesAsync();
        var cacheService = App.Current.DiContainer.Get<ImageCacheService>();
        await cacheService.SaveAvatarAsync(Avatar!);
    }

    public void ChangePassword(string password)
    {
        Password = password;
        using var context = new StellarisContext();
        var user = context.Entry(this).Entity;
        context.Users.Update(user);
        context.SaveChanges();
    }

    public void Deposit(decimal depositSum)
    {
        Balance += depositSum;
        using var context = new StellarisContext();
        var user = context.Entry(this).Entity;
        context.Users.Update(user);
        context.SaveChanges();
    }

    public void PurchaseGame(Game game)
    {
        if (Balance < game.Cost)
            throw new InvalidOperationException(
                "Cannot purchase a game that costs much more you have on your balance");

        // confirm purchase
        // update user game list (Library prop)
    }

#if DEBUG
    public static Account GetFirst()
    {
        using var context = new StellarisContext();
        return context.Users.First();
    }
#endif
    public async Task<byte[]?> GetAvatarAsync()
    {
        // get image from cache or db by guid
        // return its data
        var cacheService = App.Current.DiContainer.Get<ImageCacheService>();
        var bytes = await cacheService.LoadAvatarAsync(AvatarGuid);
        if (AvatarGuid is { } && bytes is null)
        {
            await using var context = new StellarisContext();
            var user = context.Entry(this).Entity;
            context.Users.Attach(user);
            await context.Entry(user).Reference(account => account.Avatar).LoadAsync();
            bytes = Avatar!.Data;
            await cacheService.SaveAvatarAsync(Avatar);
        }

        return bytes;
    }
}