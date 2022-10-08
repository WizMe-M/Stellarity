using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using HanumanInstitute.MvvmDialogs;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Navigation.Event;
using Stellarity.Services.Accounting;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class GameShopViewModel : ViewModelBase, IAsyncImageLoader
{
    private readonly MainViewModel _windowOwner;
    private readonly NavigationPublisher _navigator;
    private readonly IDialogService _dialog;

    public GameShopViewModel(AccountingService accountingService, MainViewModel windowOwner,
        NavigationPublisher navigator, IDialogService dialogService)
    {
        _windowOwner = windowOwner;
        _navigator = navigator;
        _dialog = dialogService;
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

    [RelayCommand]
    private void GoToAddGame()
    {
        var view = new AddGameView();
        var vm = new AddGameViewModel(_windowOwner, _dialog, _navigator);
        view.InitializeViewModelAsync(vm);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }
}