using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Game : SingleImageHolderModel<GameEntity>
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
        // todo: replace with SingleIHM.SetImageAsync(byte[], string);
        // Entity.UpdateCover(newCover.Entity);
        // Cover = new Image(Entity.Cover!);
    }

    public static IEnumerable<Game> GetAllShop()
    {
        var gameEntities = GameEntity.GetAll();
        return gameEntities.Select(entity => new Game(entity));
    }

    public static async Task AddNewAsync(string title, string description, string developer, decimal cost,
        byte[] coverData)
    {
        var entity = await GameEntity.AddAsync(title, description, developer, cost);
        var game = new Game(entity);
        await game.SetImageAsync(coverData, entity.Title);
    }
}