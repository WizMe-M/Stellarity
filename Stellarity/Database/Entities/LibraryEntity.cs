namespace Stellarity.Database.Entities;

public partial class LibraryEntity
{
    public int AccountId { get; set; }
    public int GameId { get; set; }
    public DateTime PurchaseDate { get; set; }

    public virtual GameEntity Game { get; set; } = null!;
    public virtual AccountEntity Account { get; set; } = null!;
}