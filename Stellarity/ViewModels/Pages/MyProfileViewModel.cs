using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.Pages;

public class MyProfileViewModel : PageViewModel, IAsyncImageLoader
{
    public MyProfileViewModel(Account user)
    {
        User = user;
    }

    public Bitmap? Avatar { get; private set; }
    public Account User { get; }
    public string CommentText { get; set; } = string.Empty;
    public ObservableCollection<Comment> Comments { get; } = new();

    public ICommand GoEditProfile { get; }

    public ICommand SendComment { get; }

    public Task<Bitmap?> LoadAsync()
    {
        var bm = User.Avatar?.Data.ToBitmap();
        Avatar = bm ?? Avatar;
        return Task.FromResult(bm);
    }
}