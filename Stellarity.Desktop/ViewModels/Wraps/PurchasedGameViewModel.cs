using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Models;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Wraps;

public partial class PurchasedGameViewModel : ViewModelBase, IAsyncLoader
{
    private readonly NavigationPublisher _navigator;

    [ObservableProperty] private Bitmap? _cover;

    public PurchasedGameViewModel(Key gameKey, NavigationPublisher navigator)
    {
        GameKey = gameKey;
        _navigator = navigator;
        Cover = GameKey.Game.GetImageBitmapOrDefault();
    }

    public Key GameKey { get; }

    public async Task LoadAsync()
    {
        var bitmap = await GameKey.Game.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }

    [RelayCommand]
    private void OpenGamePage()
    {
        var view = new GamePageView(GameKey.Game, _navigator);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }
}