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
    public AccountEntity? Account { get; set; }

    public static bool GameHasFreeKeys(int gameId) => GetGameFreeKeys(gameId).Any();

#if DEBUG
    public static IEnumerable<KeyEntity> GetAllGameKeys(int gameId)
    {
        using var context = new StellarityContext();
        var keys = context.Keys
            .Include(key => key.Game)
            .Include(key => key.Account)
            .Where(key => key.GameId == gameId);
        return keys.ToArray();
    }
#endif
    
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
        return keys.ToArray();
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