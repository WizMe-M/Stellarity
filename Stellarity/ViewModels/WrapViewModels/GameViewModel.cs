using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.WrapViewModels;

[ObservableObject]
public partial class GameViewModel : IAsyncImageLoader
{
    [ObservableProperty]
    private Bitmap? _cover;

    public GameViewModel(Game instance)
    {
        Instance = instance;
        _cover = Image.OpenDefaultImage();
    }

    public Game Instance { get; }

    public async Task LoadAsync()
    {
        var bytes = await Instance.GetCoverAsync();
        var bm = bytes.ToBitmap();
        Cover = bm ?? Cover;
    }
}