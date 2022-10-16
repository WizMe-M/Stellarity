using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Wraps;

public partial class AccountRowViewModel : ViewModelBase, IAsyncLoader
{
    private readonly ReadOnlyCollection<string> _banButtonStrings = new(new[] { "Ban", "Unban" });

    [ObservableProperty]
    private Bitmap _avatar = null!;

    [ObservableProperty]
    private bool _isBanned;

    [ObservableProperty]
    private string _banButtonString = string.Empty;

    public AccountRowViewModel(Account user, bool canBanUsers)
    {
        User = user;
        CanBanUsers = canBanUsers;
        ChangeBanStatus();
        Avatar = User.GetImageBitmapOrDefault();
    }

    public Account User { get; }

    public bool CanBanUsers { get; }

    [RelayCommand]
    private void ToggleBan()
    {
        User.ToggleBan();
        ChangeBanStatus();
    }

    public async Task LoadAsync()
    {
        var bitmap = await User.GetImageBitmapAsync();
        if (bitmap is { }) Avatar = bitmap;
    }


    private void ChangeBanStatus()
    {
        IsBanned = User.IsBanned;
        BanButtonString = User.IsBanned ? _banButtonStrings[1] : _banButtonStrings[0];
    }
}