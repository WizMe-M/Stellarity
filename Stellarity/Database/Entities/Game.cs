using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Stellarity.Services;

namespace Stellarity.Database.Entities;

public partial class Game
{
    public Game()
    {
        Libraries = new HashSet<Library>();
    }

    private Game(string name, string description, string developer, decimal cost)
    {
        Name = name;
        Description = description;
        Developer = developer;
        Cost = cost;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public string Developer { get; set; } = null!;

    public decimal Cost { get; set; }

    // sets now() by default
    public DateTime AddDate { get; set; }

    public Guid? CoverGuid { get; set; }

    public virtual Image? Cover { get; set; }
    public virtual ICollection<Library> Libraries { get; set; }

    public static IEnumerable<Game> GetAll()
    {
        using var context = new StellarisContext();
        var games = context.Games.ToArray();
        return games.OrderByDescending(game => game.AddDate).ThenBy(game => game.Name);
    }

    public static void Add(string name, string description, string developer, decimal cost)
    {
        using var context = new StellarisContext();
        var game = new Game(name, description, developer, cost);
        context.Games.Add(game);
        context.SaveChanges();
    }

    public static async Task AddAsync(string name, string description, string developer,
        decimal cost, byte[] coverData)
    {
        await using var context = new StellarisContext();
        var game = new Game(name, description, developer, cost);
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();
        game = context.Entry(game).Entity;
        var img = await Image.AddFromAsync(game, coverData);
        game.Cover = img;
        context.Games.Update(game);
        await context.SaveChangesAsync();
    }

    public async Task<byte[]?> GetCoverAsync()
    {
        var cacheService = App.Current.DiContainer.Get<ImageCacheService>();
        var bytes = await cacheService.LoadAvatarAsync(CoverGuid);
        if (CoverGuid is { } && bytes is null)
        {
            await using var context = new StellarisContext();
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
        await using var context = new StellarisContext();
        var game = await context.Games.FirstOrDefaultAsync(game => game.Name == name);
        return game is { };
    }
}