using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public sealed partial class AccountEntity : SingleImageHolderEntity
{
    public AccountEntity()
    {
        CommentWhereIsAuthor = new HashSet<CommentEntity>();
        CommentWhereIsProfile = new HashSet<CommentEntity>();
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

    public RoleEntity Role { get; set; } = null!;
    public ICollection<CommentEntity> CommentWhereIsAuthor { get; set; }
    public ICollection<CommentEntity> CommentWhereIsProfile { get; set; }
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
            .Include(user => user.SingleImageEntity)
            .Include(user => user.Library)
            .FirstOrDefault(user => user.Email == email && user.Password == password);
    }

    public static AccountEntity? Find(string email)
    {
        using var context = new StellarityContext();
        return context.Accounts
            .Include(user => user.Role)
            .Include(user => user.SingleImageEntity)
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
    public static AccountEntity GetAdmin()
    {
        using var context = new StellarityContext();
        return context.Accounts.First(account => account.Id == 1);
    }
#endif

    public async Task LoadLibraryAsync()
    {
        await using var context = new StellarityContext();
        var entry = context.Accounts.Attach(this);
        await entry
            .Collection(account => account.Library)
            .Query().OfType<LibraryEntity>()
            .Include(library => library.Game)
            .LoadAsync();
    }

    public async Task PurchaseGameAsync(GameEntity game)
    {
        await using var context = new StellarityContext();
        context.Accounts.Attach(this);
        Balance -= game.Cost;
        var lib = new LibraryEntity
        {
            AccountId = Id,
            GameId = game.Id
        };
        context.Libraries.Add(lib);
        context.Accounts.Update(this);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentEntity>> LoadProfileCommentsAsync()
    {
        await using var context = new StellarityContext();
        return await context.Comments
            .Include(comment => comment.Author)
            .Include(comment => comment.Profile)
            .Where(comment => comment.ProfileId == Id)
            .ToArrayAsync();
    }

    public void LoadAvatar()
    {
        using var context = new StellarityContext();
        var entry = context.Accounts.Attach(this);
        entry.Reference(entity => entity.SingleImageEntity).Load();
    }

    public void UpdateProfileInfo(string nickname, string about)
    {
        using var context = new StellarityContext();
        var entity = context.Accounts.Attach(this).Entity;
        entity.Nickname = nickname;
        entity.About = about;
        context.Accounts.Update(entity);
        context.SaveChanges();
    }

    public static int GetAccountsCount(in int accountsByTime)
    {
        using var context = new StellarityContext();
        var totalCount = context.Accounts.Count();
        return totalCount / accountsByTime;
    }

    public static async Task<IEnumerable<AccountEntity>> GetAccountsAsync(int accountsByTime, int skipRows)
    {
        await using var context = new StellarityContext();
        var accounts = context.Accounts
            .Include(entity => entity.Role)
            .Include(entity => entity.SingleImageEntity)
            .Include(entity => entity.CommentWhereIsProfile)
            .Skip(skipRows).Take(accountsByTime);

        return accounts.ToArray();
    }
}