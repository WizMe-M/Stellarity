using Stellarity.Database.Entities;

namespace Stellarity.Tests;

public class DatabaseTests
{
    [Test]
    public void EntitySetImage()
    {
        var entity = AccountEntity.GetAdmin();
        entity.SetImage(new byte[] { 2, 4, 0, 8 });
    }

    [Test]
    public void EntityLoadImage()
    {
        var entity = AccountEntity.GetAdmin();
        entity.LoadImage();
    }

    [Test]
    public void AddKey()
    {
        var gameId = 1;
        var key = "AAAAA-AAAAA-AAAAA";
        var added = KeyEntity.TryAddKey(gameId, key);
        Assert.That(added);
    }

    [Test]
    public void GetGameKeys()
    {
        var game = GameEntity.ResolveFrom(1)!;
        var keysCount = KeyEntity.GetGameFreeKeys(game.Id).Count();
        Assert.That(keysCount, Is.EqualTo(1));
    }

    [Test]
    public void GetKey()
    {
        var game = GameEntity.ResolveFrom(1)!;
        var key = KeyEntity.NextKeyOrDefault(game.Id);
        Assert.That(key, Is.Not.Null);
    }

    [Test]
    public void GameHasAnyKeys()
    {
        var game = GameEntity.ResolveFrom(1)!;
        var hasFreeKeys = game.HasFreeKeys();
        Assert.That(hasFreeKeys, Is.True);
    }

    [Test]
    public void PurchaseGame()
    {
        var userId = 1;
        var game = GameEntity.ResolveFrom(1)!;
        var key = KeyEntity.NextKeyOrDefault(game.Id);
        key?.SetKeyPurchased(userId);
        Assert.That(key is { AccountId: { } });
    }
}