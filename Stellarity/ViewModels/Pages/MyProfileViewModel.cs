using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.Pages;

public class MyProfileViewModel : PageViewModel, IAsyncImageLoader
{
    public MyProfileViewModel(Account user)
    {
        User = user;

        var canSendComment = this.WhenAnyValue(model => model.CommentText,
            s => !string.IsNullOrWhiteSpace(s));
        SendComment = ReactiveCommand.Create(() =>
        {
            var comment = new Comment(User, User, CommentText);
            Comments.Add(comment);
        }, canSendComment);
        GoEditProfile = ReactiveCommand.Create(() => { });
    }

    public Account User { get; }

    [Reactive]
    public Bitmap? Avatar { get; private set; }

    [Reactive]
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