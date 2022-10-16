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

public partial class LibraryGameViewModel : ViewModelBase, IAsyncLoader
{
    private readonly NavigationPublisher _navigator;

    [ObservableProperty]
    private Bitmap? _cover;

    public LibraryGameViewModel(LibraryGame game, NavigationPublisher navigator)
    {
        Game = game;
        _navigator = navigator;
        Cover = Game.GetImageBitmapOrDefault();
    }

    public LibraryGame Game { get; }


    public async Task LoadAsync()
    {
        var bitmap = await Game.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }


    [RelayCommand]
    private void OpenGamePage()
    {
        var view = new GamePageView(Game, _navigator);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }
}