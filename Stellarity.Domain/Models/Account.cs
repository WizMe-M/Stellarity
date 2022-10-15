using Ninject;
using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Registration;
using Stellarity.Domain.Services;

namespace Stellarity.Domain.Models;

public class Account : SingleImageHolderModel<AccountEntity>
{
    public Account(AccountEntity entity) : base(entity)
    {
        Email = Entity.Email;
        Nickname = Entity.Nickname ?? Entity.Email;
        Password = HashedPassword.FromEncrypted(Entity.Password);
        About = Entity.About ?? "";
        Balance = Entity.Balance;
        IsBanned = Entity.Deleted;
        RegistrationDate = Entity.RegistrationDate;
        Role = (Roles)Entity.RoleId;
    }

    public bool CanAddGames => Role == Roles.Administrator;
    public bool CanEditGames => Role == Roles.Administrator;

    public string Email { get; }
    public string Nickname { get; private set; }
    public HashedPassword Password { get; private set; }
    public string About { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime RegistrationDate { get; }
    public bool IsBanned { get; private set; }
    public Roles Role { get; }

    public IEnumerable<LibraryGame> Library { get; private set; } = ArraySegment<LibraryGame>.Empty;

    public bool HasAvatar => Entity.AvatarGuid is { };
    public bool IsNicknameSet => Entity.Nickname != null;

    public static async Task<AuthorizationResult> AuthorizeAsync(string email, string password)
    {
        var entity = AccountEntity.Find(email, password);
        if (entity is null) return AuthorizationResult.NoSuchUser();
        if (entity.Deleted) return AuthorizationResult.UserWasBanned();

        var account = new Account(entity);
        await account.RefreshLibraryAsync();
        return AuthorizationResult.Success(account);
    }

    public async Task RefreshLibraryAsync()
    {
        await Entity.LoadLibraryAsync();
        var games = Entity.Library.Select(library =>
            new LibraryGame(library.Game, library.PurchaseDate));
        Library = games;
    }

    public static RegistrationResult RegisterPlayer(string email, string password)
    {
        var exists = AccountEntity.Exists(email);
        if (exists) return RegistrationResult.Fail();

        var entity = AccountEntity.Register(email, password, (int)Roles.Player);
        var account = new Account(entity);
        return RegistrationResult.Success(account);
    }

    public static RegistrationResult RegisterAdministrator(string email, string password)
    {
        var exists = AccountEntity.Exists(email);
        if (exists) return RegistrationResult.Fail();

        var entity = AccountEntity.Register(email, password, (int)Roles.Administrator);
        var account = new Account(entity);
        return RegistrationResult.Success(account);
    }

    public void ApplySatisfiedPassword(in HashedPassword password)
    {
        Entity.UpdatePassword(password.Password);
        Password = HashedPassword.FromEncrypted(Entity.Password);
    }

    /// <summary>
    /// Deposits specified amount on user balance
    /// </summary>
    /// <param name="depositionAmount">Amount of deposition, that more than zero</param>
    public void Deposit(decimal depositionAmount)
    {
        Entity.UpdateBalance(depositionAmount);
        Balance = Entity.Balance;
    }

    /// <returns>Does user satisfy game's purchasing requirements</returns>
    public bool CheckCanPurchaseGame(Game game) => Balance >= game.Cost;

    /// <summary>
    /// Purchases specified game and add it to user's library
    /// </summary>
    /// <param name="game">Game that can be purchased by user - see <see cref="CheckCanPurchaseGame"/></param>
    public async Task PurchaseGameAsync(Game game)
    {
        if (!CheckCanPurchaseGame(game))
            throw new InvalidOperationException("User can't purchase this game - not enough balance");
        await Entity.PurchaseGameAsync(game.Entity);
        Balance = Entity.Balance;
        await RefreshLibraryAsync();
    }

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


    public bool IsIdenticalWith(Account acc) => acc.Entity.Id == Entity.Id;
    public bool IsMyProfile(Account profile) => profile.Entity.Id == Entity.Id;

    public async Task<byte[]> GetAvatarAsync()
    {
        if (!HasAvatar) return Array.Empty<byte>();

        var imageCacheService = DiContainingService.Kernel.Get<ImageCacheService>();
        var imageData = await imageCacheService.LoadImageAsync(Entity.AvatarGuid);
        if (imageData is null)
        {
            Entity.LoadAvatar();
            imageData = Entity.Avatar!.Data;
        }

        return imageData;
    }

    public Task EditProfileInfoAsync(string nickname, string about)
    {
        Entity.UpdateProfileInfo(nickname, about);
        Nickname = nickname;
        About = about;
        return Task.CompletedTask;
    }

    public async Task<bool> CheckGameWasPurchasedAsync(Game game)
    {
        await RefreshLibraryAsync();
        return Library.Any(g => g.Title == game.Title);
    }
}