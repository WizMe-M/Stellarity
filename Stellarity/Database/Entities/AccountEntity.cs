using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public sealed partial class AccountEntity : SingleImageHolderEntity
{
    public AccountEntity()
    {
        CommentWhereIsAuthor = new HashSet<CommentEntity>();
        CommentWhereIsProfile = new HashSet<CommentEntity>();
    }

    public AccountEntity(string email, string password, int roleId) : this()
    {
        Email = email;
        Password = password;
        Role = (Roles)roleId;
    }

    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Nickname { get; set; }
    public string Password { get; set; } = null!;
    public string? About { get; set; }
    public decimal Balance { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool Banned { get; set; }
    public bool Activated { get; set; }
    public Roles Role { get; set; }

    public ICollection<CommentEntity> CommentWhereIsAuthor { get; set; }
    public ICollection<CommentEntity> CommentWhereIsProfile { get; set; }
    public ICollection<KeyEntity> Keys { get; set; }

    public static bool Exists(string email)
    {
        using var context = new StellarityContext();
        var user = context.Accounts.FirstOrDefault(u => u.Email == email);
        return user is { };
    }

    public static async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        await using var context = new StellarityContext();
        var user = await context.Accounts.FirstOrDefaultAsync(
            account => account.Email == email, cancellationToken: cancellationToken);
        return user is { };
    }

    public static AccountEntity? Find(string email, string password)
    {
        using var context = new StellarityContext();
        return context.Accounts
            .Include(user => user.SingleImageEntity)
            .Include(user => user.Keys)
            .FirstOrDefault(user => user.Email == email && user.Password == password);
    }

    public static AccountEntity? Find(string email)
    {
        using var context = new StellarityContext();
        return context.Accounts
            .Include(user => user.SingleImageEntity)
            .Include(user => user.Keys)
            .FirstOrDefault(user => user.Email == email);
    }

    public static AccountEntity Register(string email, string password, int roleId)
    {
        using var context = new StellarityContext();
        var gamer = new AccountEntity(email, password, roleId);
        context.Accounts.Add(gamer);
        context.SaveChanges();
        return gamer;
    }

    public static async Task<AccountEntity> RegisterAsync(string email, string password, int roleId)
    {
        await using var context = new StellarityContext();
        var user = new AccountEntity(email, password, roleId);
        context.Accounts.Add(user);
        await context.SaveChangesAsync();
        return user;
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
        context.Accounts.Attach(this);
        context.Accounts.Update(this);
        context.SaveChanges();
    }

#if DEBUG
    public static AccountEntity GetAdmin()
    {
        using var context = new StellarityContext();
        return context.Accounts.First(account => account.Id == 1);
    }
#endif

    public IEnumerable<KeyEntity> GetPurchasedKeys() => KeyEntity.GetUserPurchasedKeys(Id);

    public async Task<KeyEntity> PurchaseKeyForGameAsync(GameEntity game)
    {
        await using var context = new StellarityContext();
        context.Accounts.Attach(this);
        Balance -= game.Cost;
        context.Accounts.Update(this);

        var key = game.NextKeyOrDefault()!;
        key.SetKeyPurchased(Id);
        context.Keys.Update(key);
        await context.SaveChangesAsync();

        return key;
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
            .Include(entity => entity.SingleImageEntity)
            .Include(entity => entity.CommentWhereIsProfile)
            .Skip(skipRows).Take(accountsByTime);

        return accounts.ToArray();
    }

    public void SetBanStatus(bool banStatus)
    {
        using var context = new StellarityContext();
        context.Accounts.Attach(this);
        Banned = banStatus;
        context.Accounts.Update(this);
        context.SaveChanges();
    }

    public IEnumerable<GameEntity> GetNotPurchasedGames()
    {
        var keys = KeyEntity.GetUserPurchasedKeys(Id);
        var games = GameEntity.GetAll();
        if (!keys.Any())
        {
            foreach (var gameEntity in games)
                yield return gameEntity;
        }

        foreach (var game in games)
        {
            if (keys.All(key => key.GameId != game.Id))
                yield return game;
        }
    }

    public void SetActivatedStatus(bool status)
    {
        using var context = new StellarityContext();
        context.Accounts.Attach(this);
        Activated = status;
        context.Accounts.Update(this);
        context.SaveChanges();
    }
}