using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Image;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class MyProfileViewModel : ViewModelBase, IAsyncLoader
{
    [ObservableProperty]
    private Account _user = null!;

    [ObservableProperty]
    private string _nickname = null!;

    [ObservableProperty]
    private string _aboutSelf = null!;

    [ObservableProperty]
    private Bitmap? _avatar;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SendCommentCommand))]
    private string _commentText = string.Empty;

    public MyProfileViewModel(AccountingService service)
    {
        User = service.AuthorizedUser!;
        Nickname = User.Nickname;
        AboutSelf = User.About;
        Avatar = ImagePlaceholder.GetBitmap();
    }

    public ObservableCollection<CommentViewModel> Comments { get; } = new();

    public bool CanComment => !string.IsNullOrWhiteSpace(CommentText);

    [RelayCommand(CanExecute = nameof(CanComment))]
    private async Task SendCommentAsync()
    {
        User.LeaveComment(CommentText, User);
        await RefreshComments();
        CommentText = string.Empty;
    }

    public async Task LoadAsync()
    {
        Nickname = User.Nickname;
        AboutSelf = User.About;

        var bitmap = await User.GetImageBitmapAsync();
        if (bitmap is { }) Avatar = bitmap;

        await RefreshComments();
    }

    private async Task RefreshComments()
    {
        Comments.Clear();
        var comments = await User.GetComments();
        foreach (var comment in comments) 
            Comments.Add(new CommentViewModel(comment, User));
    }
}