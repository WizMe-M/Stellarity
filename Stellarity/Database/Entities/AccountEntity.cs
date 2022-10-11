using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public sealed partial class AccountEntity : IEntity
{
    public AccountEntity()
    {
        CommentAuthors = new HashSet<CommentEntity>();
        CommentProfiles = new HashSet<CommentEntity>();
        Library = new HashSet<LibraryEntity>();
    }

    public AccountEntity(string email, string password) : this()
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

    public ImageEntity? Avatar { get; set; }
    public RoleEntity Role { get; set; } = null!;
    public ICollection<CommentEntity> CommentAuthors { get; set; }
    public ICollection<CommentEntity> CommentProfiles { get; set; }
    public ICollection<LibraryEntity> Library { get; set; }

    public static bool Exists(string email)
    {
        using var context = new StellarityContext();
        var user = context.Accounts.FirstOrDefault(u => u.Email == email);
        return user is { };
    }

    public static AccountEntity? Find(string email, string password)
    {
        using var context = new StellarityContext();
        return context.Accounts
            .Include(user => user.Role)
            .Include(user => user.Avatar)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email && user.Password == password);
    }

    public static AccountEntity? Find(string email)
    {
        using var context = new StellarityContext();
        return context.Accounts
            .Include(user => user.Role)
            .Include(user => user.Avatar)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email);
    }

    public static AccountEntity Register(string email, string password, int roleId)
    {
        using var context = new StellarityContext();
        var gamer = new AccountEntity(email, password)
        {
            RoleId = roleId
        };
        context.Accounts.Add(gamer);
        context.SaveChanges();
        context.Entry(gamer).Reference(u => u.Role).Load();
        return gamer;
    }

    public async Task SaveChangesAsync()
    {
        if (Nickname?.Trim().Length == 0) Nickname = null;
        if (About?.Trim().Length == 0) About = null;

        await using var context = new StellarityContext();
        var user = context.Entry(this).Entity;
        context.Accounts.Update(user);
        await context.SaveChangesAsync();
    }

    // public async Task ChangeAvatarAsync(byte[]? avatarData)
    // {
    //     if (avatarData is null) return;
    //
    //     await using var context = new StellarityContext();
    //     var user = context.Entry(this).Entity;
    //     context.Accounts.Attach(user);
    //     if (user.AvatarGuid is null)
    //     {
    //         var avatar = new ImageEntity(user.Email, avatarData);
    //         user.Avatar = avatar;
    //         context.Accounts.Update(user);
    //     }
    //     else
    //     {
    //         if (user.Avatar is null)
    //             await context.Entry(user).Reference(account => account.Avatar).LoadAsync();
    //         user.Avatar!.Data = avatarData;
    //         context.Images.Update(user.Avatar!);
    //     }
    //
    //     await context.SaveChangesAsync();
    //     var cacheService = DiContainingService.Kernel.Get<ImageCacheService>();
    //     await cacheService.SaveAvatarAsync(Avatar!);
    // }

    public void UpdatePassword(string password)
    {
        Password = password;
        using var context = new StellarityContext();
        var user = context.Entry(this).Entity;
        context.Accounts.Update(user);
        context.SaveChanges();
    }

    public void UpdateBalance(decimal depositSum)
    {
        Balance += depositSum;
        using var context = new StellarityContext();
        var user = context.Entry(this).Entity;
        context.Accounts.Update(user);
        context.SaveChanges();
    }

#if DEBUG
    public static AccountEntity GetFirst()
    {
        using var context = new StellarityContext();
        return context.Accounts.First(u => u.RoleId == 1);
    }
#endif

    // public async Task<byte[]?> GetAvatarAsync()
    // {
    //     // get image from cache or db by guid
    //     // return its data
    //     var cacheService = DiContainingService.Kernel.Get<ImageCacheService>();
    //     var bytes = await cacheService.LoadAvatarAsync(AvatarGuid);
    //     if (AvatarGuid is { } && bytes is null)
    //     {
    //         await using var context = new StellarityContext();
    //         var user = context.Entry(this).Entity;
    //         context.Accounts.Attach(user);
    //         await context.Entry(user).Reference(account => account.Avatar).LoadAsync();
    //         bytes = Avatar!.Data;
    //         await cacheService.SaveAvatarAsync(Avatar);
    //     }
    //
    //     return bytes;
    // }

    public async Task LoadLibraryAsync()
    {
        await using var context = new StellarityContext();
        await context.Entry(this)
            .Collection(acc => acc.Library)
            .LoadAsync();
    }

    public async Task PurchaseGameAsync(GameEntity game)
    {
        await using var context = new StellarityContext();

        var entity = context.Entry(this).Entity;
        entity.Balance -= game.Cost;
        var lib = new LibraryEntity()
        {
            AccountId = Id,
            GameId = game.Id
        };
        context.Libraries.Add(lib);
        context.Accounts.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentEntity>> LoadCommentsForAsync(AccountEntity profile)
    {
        await using var context = new StellarityContext();
        return await context.Comments
            .Where(comment => comment.ProfileId == profile.Id)
            .ToArrayAsync();
    }
}