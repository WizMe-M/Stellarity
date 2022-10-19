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
}