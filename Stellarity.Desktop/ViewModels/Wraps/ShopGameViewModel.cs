using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.Models;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Models;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Wraps;

[ObservableObject]
public partial class ShopGameViewModel : IAsyncImageLoader
{
    private readonly NavigationPublisher _navigator;

    [ObservableProperty]
    private Bitmap? _cover;

    public ShopGameViewModel(Game instance, NavigationPublisher navigator)
    {
        _navigator = navigator;
        Instance = instance;
        _cover = Instance.TryGetImageBytes().ToBitmap() ?? ImagePlaceholder.GetBitmap();
    }

    public Game Instance { get; }

    [RelayCommand]
    private void OpenGamePage()
    {
        var view = new GamePageView(Instance);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }

    public async Task LoadAsync()
    {
        var bitmap = await Instance.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }
}