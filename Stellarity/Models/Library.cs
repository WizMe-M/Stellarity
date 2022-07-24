using System;

namespace Stellarity.Models;

public partial class Library
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime PurchaseDate { get; set; }

    public virtual Game Game { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}