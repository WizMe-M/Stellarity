using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Services.Accounting;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class GameShopViewModel : ViewModelBase, IAsyncImageLoader
{
    public GameShopViewModel(AccountingService accountingService)
    {
        Authorized = accountingService.AuthorizedAccount!;

        var games = Game.GetAll();
        AllGames.AddRange(games.Select(g => new GameViewModel(g)));
    }

    public ObservableCollection<GameViewModel> AllGames { get; } = new();

    public Account Authorized { get; }

    public async Task LoadAsync()
    {
        foreach (var game in AllGames) await game.LoadAsync();
    }
}