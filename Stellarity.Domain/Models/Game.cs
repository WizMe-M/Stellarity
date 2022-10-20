using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Game : SingleImageHolderModel<GameEntity>
{
    public Game(GameEntity entity) : base(entity)
    {
    }

    public string Title => Entity.Title;
    public string Description => Entity.Description;
    public string Developer => Entity.Developer;
    public decimal Cost => Entity.Cost;
    public DateTime AddedInShopDate => Entity.AddDate;

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

    public async Task EditAsync(string title, string description, string developer, decimal cost, byte[]? coverData)
    {
        Entity.UpdateInfo(title, description, developer, cost);

        if (coverData is { }) 
            await SetImageAsync(coverData, title);
    }
}