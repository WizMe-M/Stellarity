using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Image;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class GamePageViewModel : ViewModelBase
{
    private readonly Game _game;
    private readonly NavigationPublisher _navigator;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private decimal _cost;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _developer = string.Empty;

    [ObservableProperty]
    private Bitmap _cover = null!;

    [ObservableProperty]
    private bool _wasPurchased;

    [ObservableProperty]
    private DateTime? _purchaseDate;

    public GamePageViewModel(Game game, NavigationPublisher navigator)
    {
        _game = game;
        _navigator = navigator;
        AddedInShop = _game.AddedInShopDate;
        Cover = ImagePlaceholder.GetBitmap();
        SetInfo();

        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        User = accountingService.AuthorizedUser!;
    }

    public Account User { get; }

    public DateTime AddedInShop { get; }

    public bool CanPurchase => User.CheckCanPurchaseGame(_game);

    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task PurchaseAsync()
    {
        var dialogService = DiContainingService.Kernel.Get<IDialogService>();
        var vm = DiContainingService.Kernel.Get<MainViewModel>();
        var result = await dialogService.ShowMessageBoxAsync(vm, "Confirm purchase, please",
            button: MessageBoxButton.YesNo, defaultResult: false);

        if (result == true)
        {
            await User.PurchaseGameAsync(_game);
            await UpdatePurchased();
        }
    }

    [RelayCommand]
    private void NavigateToLibrary()
    {
        var view = new LibraryView { ViewModel = new LibraryViewModel(_navigator) };
        _navigator.RaiseNavigated(this, NavigatedEventArgs.ReplaceLast(view));
    }

    [RelayCommand]
    private void NavigateToEditGame()
    {
        var view = new EditGameView { ViewModel = new EditGameViewModel(_game, _navigator) };
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }

    private async Task UpdatePurchased()
    {
        WasPurchased = await User.CheckGameWasPurchasedAsync(_game);
        if (WasPurchased)
        {
            var purchased = User.Library.First(g => g.Title == _game.Title);
            PurchaseDate = purchased.PurchaseDate;
        }
    }

    public async Task UpdatePageAsync()
    {
        SetInfo();
        await UpdateCover();
        await UpdatePurchased();
    }

    private void SetInfo()
    {
        Title = _game.Title;
        Cost = _game.Cost;
        Description = _game.Description;
        Developer = _game.Developer;
    }

    private async Task UpdateCover()
    {
        var gameCover = await _game.GetImageBitmapAsync();
        Cover = gameCover ?? ImagePlaceholder.GetBitmap();
    }
}