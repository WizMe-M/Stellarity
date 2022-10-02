using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Stellarity.Avalonia.Extensions;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Image = Stellarity.Avalonia.Models.Image;

namespace Stellarity.Desktop.ViewModels.Wraps;

[ObservableObject]
public partial class GameViewModel : IAsyncImageLoader
{
    [ObservableProperty]
    private Bitmap? _cover;

    public GameViewModel(Game instance)
    {
        Instance = instance;
        _cover = Image.GetPlaceholderBitmap();
    }

    public Game Instance { get; }

    public async Task LoadAsync()
    {
        var bytes = await Instance.GetCoverAsync();
        var bm = bytes.ToBitmap();
        Cover = bm ?? Cover;
    }
}