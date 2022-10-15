using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ninject;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.Models;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class GamePageViewModel : ViewModelBase
{
    private readonly Game _game;

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

    public GamePageViewModel(Game game)
    {
        _game = game;
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
        // TODO: show purchase confirmation dialog Yes/No
        if (true)
        {
            await User.PurchaseGameAsync(_game);
        }
    }

    public Task UpdateGameInfoAsync()
    {
        SetInfo();
        return UpdateCover();
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