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
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class GameShopViewModel : ViewModelBase, IAsyncLoader
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
        Authorized = accountingService.AuthorizedUser!;
    }

    public ObservableCollection<ShopGameViewModel> AllGames { get; } = new();

    public Account Authorized { get; }

    public async Task LoadAsync()
    {
        AllGames.Clear();
        var games = Game.GetAllShop();
        AllGames.AddRange(games.Select(g => new ShopGameViewModel(g, _navigator)));

        foreach (IAsyncLoader asyncImageLoader in AllGames)
            await asyncImageLoader.LoadAsync();
    }

    [RelayCommand]
    private void GoToAddGame()
    {
        var view = new AddGameView { ViewModel = new AddGameViewModel(_windowOwner, _dialog, _navigator) };
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }
}