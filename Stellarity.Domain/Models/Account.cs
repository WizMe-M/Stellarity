using Microsoft.EntityFrameworkCore.Infrastructure;
using Ninject;
using Stellarity.Database;
using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Email;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Services;

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
    public bool IsActivated => Entity.Activated;
    public Roles Role => Entity.Role;

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
        if (!entity.Activated) return AuthorizationResult.UserIsNotActivated();

        var account = new Account(entity);
        await account.RefreshLibraryAsync();
        return AuthorizationResult.Success(account);
    }

    public Task<IEnumerable<Key>> RefreshLibraryAsync()
    {
        var keys = Entity.GetPurchasedKeys();
        Keys = keys.Select(entity => new Key(entity));
        return Task.FromResult(Keys);
    }

    public static async Task<RegistrationResult> RegisterUserAsync(string email, string password, Roles role, 
        bool activated = false)
    {
        var exists = AccountEntity.Exists(email);
        if (exists) return RegistrationResult.AlreadyExistsWithEmail();

        var hashedPassword = HashedPassword.FromDecrypted(password).Password;
        var entity = await AccountEntity.RegisterAsync(email, hashedPassword, (int)role, activated);
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

    public async Task<Key> PurchaseGameAsync(Game game)
    {
        if (!CheckCanPurchaseGame(game))
            throw new InvalidOperationException("Can't purchase this game");

        var purchasedKey = await Entity.PurchaseKeyForGameAsync(game.Entity);
        var chequeSenderService = DiContainingService.Kernel.Get<GameChequeSenderService>();
        await chequeSenderService.SendAsync(Email, new Key(purchasedKey));
        await RefreshLibraryAsync();
        return new Key(purchasedKey);
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

    public IEnumerable<Game> GetNotPurchasedGames() => Entity.GetNotPurchasedGames().Select(game => new Game(game));

    public static void Activate(string email)
    {
        var account = AccountEntity.Find(email);
        account?.SetActivatedStatus(true);
    }
}