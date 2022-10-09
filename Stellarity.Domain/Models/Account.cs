using Stellarity.Domain.Authorization;
using Stellarity.Domain.Registration;
using AccountEntity = Stellarity.Database.Entities.Account;

namespace Stellarity.Domain.Models;

public class Account
{
    public AccountEntity Entity { get; }

    public Account(AccountEntity entity)
    {
        Entity = entity;
        Email = Entity.Email;
        Nickname = Entity.Nickname ?? Entity.Email;
        Password = Entity.Password;
        About = Entity.About ?? "";
        IsBanned = Entity.Deleted;
        RegistrationDate = Entity.RegistrationDate;
        Role = (Roles)Entity.RoleId;
    }

    public bool CanAddGames => Role == Roles.Administrator;
    public string Email { get; private set; }
    public string Nickname { get; private set; }
    public string Password { get; private set; }
    public string About { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime RegistrationDate { get; }
    public bool IsBanned { get; private set; }
    public Roles Role { get; }

    public Image? Avatar { get; private set; }
    public IEnumerable<Game> Library { get; private set; } = ArraySegment<Game>.Empty;

    public static async Task<AuthorizationResult> AuthorizeAsync(string email, string password)
    {
        var entity = AccountEntity.Find(email, password);
        if (entity is null) return AuthorizationResult.Fail();

        var account = new Account(entity);
        await account.LoadLibraryAsync();
        return AuthorizationResult.Success(account);
    }

    private async Task LoadLibraryAsync()
    {
        await Entity.UpdateLibraryAsync();
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

    /// <summary>
    /// Applies new user password
    /// </summary>
    /// <param name="password">String that satisfies all requirements to password</param>
    public void ChangePassword(in string password)
    {
        Entity.UpdatePassword(password);
        Password = Entity.Password;
    }

    /// <summary>
    /// Deposits specified amount on user balance
    /// </summary>
    /// <param name="depositSum">Amount of deposition, that more than zero</param>
    public void Deposit(decimal depositSum)
    {
        Entity.UpdateBalance(depositSum);
        Balance = Entity.Balance;
    }

    /// <returns>Does user satisfy game's purchasing requirements</returns>
    public bool CanPurchaseGame(Game game) => Balance < game.Cost;

    /// <summary>
    /// Purchases specified game and add it to user's library
    /// </summary>
    /// <param name="game">Game that user can purchase - see <see cref="CanPurchaseGame"/></param>
    public async Task PurchaseGameAsync(Game game)
    {
        await Entity.PurchaseGameAsync(game.Entity);
        Balance = Entity.Balance;
        await LoadLibraryAsync();
    }

    public void LeaveCommentOnMyProfile(string commentText)
    {
        var comment = Comment.SendOnMyProfile(commentText, this);
    }
}