using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.Pages;

public class EditProfileViewModel : ValidatableViewModelBase, IAsyncImageLoader
{
    private readonly Account _user;

    public EditProfileViewModel(Account user)
    {
        _user = user;
        Nickname = _user.Nickname ?? _user.Email;
        AboutSelf = _user.About;

        SaveChanges = ReactiveCommand.CreateFromTask(async () => { });
        SetAvatar = ReactiveCommand.Create(() => { });
        ChangePassword = ReactiveCommand.Create(() => { });
    }

    [Reactive]
    public Bitmap? Avatar { get; private set; }

    [Reactive]
    public string Nickname { get; set; }

    [Reactive]
    public string? AboutSelf { get; set; }

    public ICommand SaveChanges { get; }
    public ICommand SetAvatar { get; }
    public ICommand ChangePassword { get; }

    public Task<Bitmap?> LoadAsync()
    {
        var bm = _user.Avatar?.Data.ToBitmap();
        Avatar = bm ?? Avatar;
        return Task.FromResult(bm);
    }
}