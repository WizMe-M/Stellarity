using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database.Entities;

public partial class GameEntity : SingleImageHolderEntity
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

    public ICollection<LibraryEntity> Libraries { get; set; }
    public ICollection<KeyEntity> Keys { get; set; }

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

    public static async Task<GameEntity> AddAsync(string name, string description, string developer,
        decimal cost)
    {
        await using var context = new StellarityContext();
        var game = new GameEntity(name, description, developer, cost);
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();
        return game;
    }

    public static async Task<bool> ExistsAsync(string title, CancellationToken cancellationToken)
    {
        await using var context = new StellarityContext();
        var game = await context.Games.FirstOrDefaultAsync(game => game.Title == title,
            cancellationToken: cancellationToken);
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
        context.Games.Attach(this);
        Title = title;
        Description = description;
        Developer = developer;
        Cost = cost;
        context.Games.Update(this);
        context.SaveChanges();
    }

    public bool HasFreeKeys() => KeyEntity.GetGameFreeKeys(Id).Any();

    public KeyEntity? NextKeyOrDefault() => KeyEntity.NextKeyOrDefault(Id);
}