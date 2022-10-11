using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Game : DomainModel<GameEntity>
{
    protected Game(GameEntity entity) : base(entity)
    {
        Title = Entity.Title;
        Description = Entity.Description;
        Developer = Entity.Developer;
        Cost = Entity.Cost;
        AddedInShopDate = Entity.AddDate;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Developer { get; private set; }
    public decimal Cost { get; private set; }

    public Image? Cover { get; private set; }

    public DateTime AddedInShopDate { get; }

    public void EditBasicInfo(in string title, in string description, in string developer, in decimal cost)
    {
        Entity.UpdateInfo(title, description, developer, cost);
        Title = Entity.Title;
        Description = Entity.Description;
        Developer = Entity.Developer;
        Cost = Entity.Cost;
    }

    public void ChangeCover(Image newCover)
    {
        Entity.UpdateCover(newCover.Entity);
        Cover = new Image(Entity.Cover!);
    }
}