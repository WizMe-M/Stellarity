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
    [ObservableProperty] private Bitmap? _cover;

    public GameViewModel(Game instance)
    {
        Instance = instance;
        _cover = ImagePlaceholder.GetBitmap();
    }

    public Game Instance { get; }

    public async Task LoadAsync()
    {
        var bytes = await Instance.GetCoverBytesAsync();
        var bm = bytes.ToBitmap();
        Cover = bm ?? Cover;
    }
}