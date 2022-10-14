using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.Models;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Wraps;

[ObservableObject]
public partial class GameViewModel : IAsyncImageLoader
{
    [ObservableProperty]
    private Bitmap? _cover;

    public GameViewModel(Game instance)
    {
        Instance = instance;
        _cover = Instance.TryGetImageBytes().ToBitmap() ?? ImagePlaceholder.GetBitmap();
    }

    public Game Instance { get; }

    public void OpenGamePage()
    {
        // TODO: make 'open game page' command
        // needs navigator
        // should it be DI-contained?
    }

    public async Task LoadAsync()
    {
        var bitmap = await Instance.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }
}