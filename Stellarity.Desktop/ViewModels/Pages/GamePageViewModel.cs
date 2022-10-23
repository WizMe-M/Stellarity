using System;
using System.Collections.Generic;
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

    public GamePageViewModel(Game game, NavigationPublisher navigator)
    {
        _game = game;
        _navigator = navigator;
        AddedInShop = _game.AddedInShopDate;
        Cover = ImagePlaceholder.GetBitmap();
        SetInfo();

        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        Visitor = accountingService.AuthorizedUser!;
    }

    public GamePageViewModel(Key gameKey, NavigationPublisher navigator) : this(gameKey.Game, navigator)
    {
        if (gameKey is { IsPurchased: false })
            throw new NotSupportedException("This ctor is available for purchased keys only");

        GameKey = gameKey;
    }

    public Account Visitor { get; }

    public DateTime AddedInShop { get; }

    public bool IsPurchased => GameKey is { IsPurchased: true };
    public Key? GameKey { get; }

    public bool CanPurchase => !IsPurchased && Visitor.CheckCanPurchaseGame(_game);

    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task PurchaseAsync()
    {
        var dialogService = DiContainingService.Kernel.Get<IDialogService>();
        var vm = DiContainingService.Kernel.Get<MainViewModel>();
        var result = await dialogService.ShowMessageBoxAsync(vm, "Confirm purchase, please",
            button: MessageBoxButton.YesNo, defaultResult: false);

        if (result == true)
        {
            var key = await Visitor.PurchaseGameAsync(_game);
            var view = new GamePageView(key, _navigator);
            _navigator.RaiseNavigated(this, NavigatedEventArgs.ReplaceLast(view));
        }
    }

    [RelayCommand]
    private async Task ImportAsync()
    {
        var dialogService = DiContainingService.Kernel.Get<IDialogService>();
        var windowOwner = DiContainingService.Kernel.Get<MainViewModel>();

        await dialogService.ShowMessageBoxAsync(windowOwner,
            "Импортируйте ключи из CSV и Excel. CSV обязательно должен иметь заголовок \"Value\". " +
            "Excel не имеет заголовка, ключи располагаются в первом столбце",
            "Инструкции к импорту", MessageBoxButton.Ok, MessageBoxImage.Information);

        var settings = new OpenFileDialogSettings
        {
            Title = "Импортировать ключи",
            AllowMultiple = false,
            Filters = new List<FileFilter>
            {
                new("Excel Book", "xlsx"),
                new("CSV File", "csv")
            }
        };
        var path = await dialogService.ShowOpenFileDialogAsync(windowOwner, settings);
        if (path is not { }) return;
        await _game.ImportKeysAsync(path);

        PurchaseCommand.NotifyCanExecuteChanged();
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

    public async Task UpdatePageAsync()
    {
        SetInfo();
        await UpdateCover();
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