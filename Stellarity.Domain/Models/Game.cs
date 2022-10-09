using GameEntity = Stellarity.Database.Entities.Game;

namespace Stellarity.Domain.Models;

public class Game
{
    public GameEntity Entity { get; }

    protected Game(GameEntity entity)
    {
        Entity = entity;
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

    
    /// <summary>
    /// Edits information about game. All parameters are satisfying requirements
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="developer"></param>
    /// <param name="cost"></param>
    public void EditBasicInfo(in string title, in string description, in string developer, in string cost)
    {
        Entity.UpdateInfo(title, description, developer, cost);
        Title = Entity.Title;
        Description = Entity.Description;
        Developer = Entity.Developer;
        Cost = Entity.Cost;
    }
}