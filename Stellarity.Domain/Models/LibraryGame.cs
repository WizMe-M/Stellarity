using Stellarity.Database.Entities;

namespace Stellarity.Domain.Models;

public class LibraryGame : Game
{
    public LibraryGame(GameEntity entity, DateTime purchaseDate) : base(entity)
    {
        PurchaseDate = purchaseDate;
    }

    public DateTime PurchaseDate { get; }
}