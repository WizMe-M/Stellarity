using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public sealed partial class KeyEntity : IEntity
{
    // PK
    public string KeyValue { get; set; }

    // both unique
    public int GameId { get; set; }
    public int? AccountId { get; set; }

    // default now()
    public DateTime? PurchaseDate { get; set; }

    public GameEntity Game { get; set; }
    public AccountEntity Account { get; set; }

    public static bool GameHasFreeKeys(int gameId) => GetGameFreeKeys(gameId).Any();

    public static IEnumerable<KeyEntity> GetGameFreeKeys(int gameId)
    {
        using var context = new StellarityContext();
        var keys = context.Keys
            .Include(key => key.Game)
            .Where(key => key.GameId == gameId && key.AccountId == null);
        return keys.ToArray();
    }

    public static IEnumerable<KeyEntity> GetUserPurchasedKeys(int userId)
    {
        using var context = new StellarityContext();
        var keys = context.Keys
            .Include(key => key.Account)
            .Include(key => key.Game)
            .Where(key => key.AccountId == userId);
        return keys;
    }

    public static KeyEntity? NextKeyOrDefault(int gameId)
    {
        using var context = new StellarityContext();
        return context.Keys.FirstOrDefault(key => key.GameId == gameId && key.AccountId == null);
    }

    public static bool TryAddKey(int gameId, string value)
    {
        using var context = new StellarityContext();
        var containsSuchKey = context.Keys.Any(key => key.KeyValue == value);
        if (containsSuchKey) return false;

        var key = new KeyEntity { KeyValue = value, GameId = gameId };
        context.Keys.Add(key);
        context.SaveChanges();
        return true;
    }

    public void SetKeyPurchased(int userId)
    {
        using var context = new StellarityContext();
        context.Keys.Attach(this);
        AccountId = userId;
        PurchaseDate = DateTime.Now;
        context.Keys.Update(this);
        context.SaveChanges();
    }
}