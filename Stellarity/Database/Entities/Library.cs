namespace Stellarity.Database.Entities;

public partial class Library
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime PurchaseDate { get; set; }

    public virtual Game Game { get; set; } = null!;
    public virtual Account Account { get; set; } = null!;
}