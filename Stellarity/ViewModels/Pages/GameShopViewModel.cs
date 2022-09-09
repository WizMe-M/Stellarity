using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.ViewModels.WrapViewModels;

namespace Stellarity.ViewModels.Pages;

public class GameShopViewModel : IAsyncImageLoader
{
    public GameShopViewModel()
    {
        var games = Game.GetAll();
        AllGames.AddRange(games.Select(g => new GameViewModel(g)));
    }

    public ObservableCollection<GameViewModel> AllGames { get; } = new();

    public async Task LoadAsync()
    {
        foreach (var game in AllGames) await game.LoadAsync();
    }
}