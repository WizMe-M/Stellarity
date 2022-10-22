using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public sealed partial class KeyEntity : IEntity
{
    public string KeyValue { get; set; } = null!;

    public int GameId { get; set; }

    public int? AccountId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public GameEntity Game { get; set; } = null!;
    public AccountEntity? Account { get; set; }

    public static IEnumerable<KeyEntity> GetGameFreeKeys(int gameId)
    {
        using var context = new StellarityContext();
        var keys = context.Keys
            .Include(key => key.Game)
            .Where(key => key.GameId == gameId && key.AccountId == null);
        return keys.ToArray();
    }

    public static KeyEntity[] GetUserPurchasedKeys(int userId)
    {
        using var context = new StellarityContext();
        var keys = context.Keys
            .Include(key => key.Account)
            .Include(key => key.Game)
            .Where(key => key.AccountId == userId);
        return keys.ToArray();
    }

    public static KeyEntity? NextKeyOrDefault(int gameId)
    {
        using var context = new StellarityContext();
        return context.Keys
            .Include(key => key.Game)
            .FirstOrDefault(key => key.GameId == gameId && key.AccountId == null);
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

    public static void ImportKeys(IEnumerable<(int gameId, string keyValue)> imported)
    {
        using var context = new StellarityContext();

        foreach (var newKey in imported)
        {
            var containsSuchKey = context.Keys.Any(key => key.KeyValue == newKey.keyValue);
            if (containsSuchKey) continue;

            var key = new KeyEntity { KeyValue = newKey.keyValue, GameId = newKey.gameId };
            context.Keys.Add(key);
        }

        context.SaveChanges();
    }

    public void SetKeyPurchased(int userId)
    {
        AccountId = userId;
        PurchaseDate = DateTime.Now;
    }

    public static bool Exists(int accountId, int gameId)
    {
        using var context = new StellarityContext();
        return context.Keys.Any(key => key.AccountId == accountId && key.GameId == gameId);
    }
}