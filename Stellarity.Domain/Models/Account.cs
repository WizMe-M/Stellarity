using Stellarity.Database;
using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Registration;

namespace Stellarity.Domain.Models;

public class Account : SingleImageHolderModel<AccountEntity>
{
    public Account(AccountEntity entity) : base(entity)
    {
    }

    public bool CanAddGames => Role == Roles.Administrator;
    public bool CanEditGames => Role == Roles.Administrator;
    public bool CanAddUsers => Role == Roles.Administrator;
    public bool CanBanUsers => Role == Roles.Administrator;

    public string Email => Entity.Email;
    public string Nickname => Entity.Nickname ?? Email;
    public HashedPassword Password => HashedPassword.FromEncrypted(Entity.Password);
    public string About => Entity.About ?? "";
    public decimal Balance => Entity.Balance;
    public DateTime RegistrationDate => Entity.RegistrationDate;
    public bool IsBanned => Entity.Banned;
    public Roles Role => Entity.Role;

    public IEnumerable<LibraryGame> Library { get; private set; } = ArraySegment<LibraryGame>.Empty;
    public IEnumerable<Key> Keys { get; private set; } = ArraySegment<Key>.Empty;

    public bool IsNicknameSet => Entity.Nickname is { };

    public static async Task<IEnumerable<Account>> GetAccountsAsync(int i, int accountsByTime)
    {
        var skipRows = i * accountsByTime;
        var accountEntities = await AccountEntity.GetAccountsAsync(accountsByTime, skipRows);
        var accounts = accountEntities.Select(entity => new Account(entity));
        return accounts;
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(string email, string password)
    {
        var entity = AccountEntity.Find(email, password);
        if (entity is null) return AuthorizationResult.NoSuchUser();
        if (entity.Banned) return AuthorizationResult.UserWasBanned();

        var account = new Account(entity);
        await account.RefreshLibraryAsync();
        return AuthorizationResult.Success(account);
    }

    public async Task<IEnumerable<Key>> RefreshLibraryAsync()
    {
        var keys = await Entity.GetPurchasedKeys();
        Keys = keys.Select(entity => new Key(entity));
        return Keys;
    }

    public static async Task<RegistrationResult> RegisterUserAsync(string email, string password, Roles role)
    {
        var exists = AccountEntity.Exists(email);
        if (exists) return RegistrationResult.AlreadyExistsWithEmail();

        var hashedPassword = HashedPassword.FromDecrypted(password).Password;
        var entity = await AccountEntity.RegisterAsync(email, hashedPassword, (int)role);
        var account = new Account(entity);
        return RegistrationResult.Success(account);
    }

    public static HashedPassword ChangePassword(string email, string password)
    {
        var entity = AccountEntity.Find(email);
        if (entity is null) throw new InvalidOperationException("User with such email doesn't exist");
        var account = new Account(entity);
        var hashed = HashedPassword.FromDecrypted(password);
        account.ApplySatisfiedPassword(hashed);
        return account.Password;
    }

    public bool IsIdenticalWith(Account acc) => acc.Entity.Id == Entity.Id;

    public bool CheckCanPurchaseGame(Game game) =>
        !CheckHasPurchasedGame(game) && game.HasFreeKeys && Balance >= game.Cost;

    public bool CheckHasPurchasedGame(Game game) => Key.WasPurchased(this, game);

    public async Task PurchaseGameAsync(Game game)
    {
        if (!CheckCanPurchaseGame(game))
            throw new InvalidOperationException("Can't purchase this game");

        await Entity.PurchaseKeyForGameAsync(game.Entity);
        // TODO: notify game cheque mailing
        await RefreshLibraryAsync();
    }

    public void ToggleBan() => Entity.SetBanStatus(!IsBanned);

    public void ApplySatisfiedPassword(in HashedPassword password) => Entity.UpdatePassword(password.Password);

    public Task EditProfileInfoAsync(string nickname, string about)
    {
        Entity.UpdateProfileInfo(nickname, about);
        return Task.CompletedTask;
    }

    public void Deposit(decimal depositionAmount) => Entity.UpdateBalance(depositionAmount);

    public async Task<IEnumerable<Comment>> GetComments()
    {
        var comments = await Entity.LoadProfileCommentsAsync();
        return comments.Select(entity => new Comment(entity));
    }

    public Comment LeaveComment(string commentText, Account profile)
    {
        var comment = IsIdenticalWith(profile)
            ? Comment.SendOnMyProfile(commentText, profile)
            : Comment.SendOnOtherProfile(commentText, profile, this);
        return comment;
    }
}