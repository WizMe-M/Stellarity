using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Key : DomainModel<KeyEntity>
{
    public Key(KeyEntity entity) : base(entity)
    {
        Game = new Game(Entity.Game);
        if (Entity.Account is null) return;
        Buyer = new Account(Entity.Account);
        IsPurchased = true;
    }


    public string Value => Entity.KeyValue;
    public Game Game { get; }
    public Account? Buyer { get; }
    public bool IsPurchased { get; }

    public DateTime? PurchaseDate => Entity.PurchaseDate;

    public static bool WasPurchased(Account account, Game game)
    {
        return KeyEntity.Exists(account.Entity.Id, game.Entity.Id);
    }
}