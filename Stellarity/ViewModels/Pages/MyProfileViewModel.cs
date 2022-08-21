using System.Drawing;
using System.Threading.Tasks;
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

    public Account User { get; }
    public Bitmap? Avatar { get; private set; }

    public Task<Bitmap?> LoadAsync()
    {
        var bm = User.Avatar?.Data.ToBitmap();
        Avatar = bm ?? Avatar;
        return Task.FromResult(bm);
    }
}