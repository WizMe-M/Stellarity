using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Wraps;

public partial class AccountRowViewModel : ViewModelBase, IAsyncLoader
{
    public Account User { get; }

    [ObservableProperty]
    private Bitmap _avatar = null!;

    public AccountRowViewModel(Account user)
    {
        User = user;
        Avatar = User.GetImageBitmapOrDefault();
    }

    public async Task LoadAsync()
    {
        var bitmap = await User.GetImageBitmapAsync();
        if (bitmap is { }) Avatar = bitmap;
    }
}