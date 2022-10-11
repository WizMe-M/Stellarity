using Microsoft.EntityFrameworkCore;
using Ninject;
using Stellarity.Services;

namespace Stellarity.Database.Entities;

public partial class GameEntity : IEntity
{
    public GameEntity()
    {
        Libraries = new HashSet<LibraryEntity>();
    }

    private GameEntity(string title, string description, string developer, decimal cost)
    {
        Title = title;
        Description = description;
        Developer = developer;
        Cost = cost;
    }

    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; }
    public string Developer { get; set; } = null!;

    public decimal Cost { get; set; }

    // sets now() by default
    public DateTime AddDate { get; set; }

    public Guid? CoverGuid { get; set; }

    public virtual ImageEntity? Cover { get; set; }
    public virtual ICollection<LibraryEntity> Libraries { get; set; }

    public static IEnumerable<GameEntity> GetAll()
    {
        using var context = new StellarityContext();
        var games = context.Games.ToArray();
        return games.OrderByDescending(game => game.AddDate).ThenBy(game => game.Title);
    }

    public static void Add(string name, string description, string developer, decimal cost)
    {
        using var context = new StellarityContext();
        var game = new GameEntity(name, description, developer, cost);
        context.Games.Add(game);
        context.SaveChanges();
    }

    public static async Task AddAsync(string name, string description, string developer,
        decimal cost, byte[] coverData)
    {
        await using var context = new StellarityContext();
        var game = new GameEntity(name, description, developer, cost);
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();
        game = context.Entry(game).Entity;
        var img = await ImageEntity.AddFromAsync(game, coverData);
        game.Cover = img;
        context.Games.Update(game);
        await context.SaveChangesAsync();
    }

    public async Task<byte[]?> GetCoverAsync()
    {
        var cacheService = DiContainingService.Kernel.Get<ImageCacheService>();
        var bytes = await cacheService.LoadAvatarAsync(CoverGuid);
        if (CoverGuid is { } && bytes is null)
        {
            await using var context = new StellarityContext();
            var game = context.Entry(this).Entity;
            context.Games.Attach(game);
            await context.Entry(game).Reference(g => g.Cover).LoadAsync();
            bytes = Cover!.Data;
            await cacheService.SaveAvatarAsync(Cover);
        }

        return bytes;
    }

    public static async Task<bool> ExistsAsync(string title)
    {
        var name = title.Trim();
        await using var context = new StellarityContext();
        var game = await context.Games.FirstOrDefaultAsync(game => game.Title == name);
        return game is { };
    }

    public static GameEntity? ResolveFrom(int gameId)
    {
        using var context = new StellarityContext();
        return context.Games.FirstOrDefault(game => game.Id == gameId);
    }

    public void UpdateInfo(in string title, in string description, in string developer, in decimal cost)
    {
        using var context = new StellarityContext();
        var entity = context.Entry(this).Entity;
        entity.Title = title;
        entity.Description = description;
        entity.Developer = developer;
        entity.Cost = cost;
        context.Games.Update(entity);
        context.SaveChanges();

        Title = entity.Title;
    }

    public void UpdateCover(ImageEntity newCover)
    {
        using var context = new StellarityContext();
        var entity = context.Entry(this).Entity;
    }
}