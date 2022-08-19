using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        decimal cost, string coverPath)
    {
        await using var context = new StellarisContext();
        var game = new Game(name, description, developer, cost);
        context.Games.Add(game);
        await context.SaveChangesAsync();

        game = await context.Games.FirstAsync(g => g.Name == game.Name);
        await Image.SaveAsync(coverPath, game);
    }
}