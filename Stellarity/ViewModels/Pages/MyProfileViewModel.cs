using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;
using Stellarity.Services.Accounting;

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
            // TODO: for testing only
            // var r = new Random();
            var same = true; //r.Next() % 2 == 0;
            var comment = same
                ? Comment.Send(CommentText, User)
                : Comment.Send(CommentText, User, new Account("other@mail.ru", ""));
            Comments.Add(comment);
        }, canSendComment);
        GoEditProfile = ReactiveCommand.Create(() => { });
    }

    public MyProfileViewModel(AccountingService accountingService)
        : this(accountingService.AuthorizedAccount!)
    {
    }

    public Account User { get; }

    [Reactive]
    public Bitmap? Avatar { get; private set; }

    [Reactive]
    public string CommentText { get; set; } = string.Empty;

    public ObservableCollection<Comment> Comments { get; } = new();

    public ICommand GoEditProfile { get; }

    public ICommand SendComment { get; }

    public async Task<Bitmap?> LoadAsync()
    {
        var bm = User.Avatar?.Data.ToBitmap();
        Avatar = bm ?? await Image.OpenDefaultImageAsync();
        return bm;
    }
}