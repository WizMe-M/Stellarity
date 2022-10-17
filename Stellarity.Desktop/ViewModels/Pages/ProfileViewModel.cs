using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ninject;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Image;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class ProfileViewModel : ViewModelBase, IAsyncLoader
{
    [ObservableProperty]
    private Account _user = null!;

    [ObservableProperty]
    private Account _profile = null!;

    [ObservableProperty]
    private string _nickname = string.Empty;

    [ObservableProperty]
    private string _aboutSelf = string.Empty;

    [ObservableProperty]
    private Bitmap? _avatar;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SendCommentCommand))]
    private string _commentText = string.Empty;

    public ProfileViewModel(Account profile)
    {
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        User = accountingService.AuthorizedUser!;
        Profile = profile;
        Nickname = Profile.Nickname;
        AboutSelf = Profile.About;
        Avatar = ImagePlaceholder.GetBitmap();
    }

    public ObservableCollection<CommentViewModel> Comments { get; } = new();

    public bool CanComment => !string.IsNullOrWhiteSpace(CommentText);

    [RelayCommand(CanExecute = nameof(CanComment))]
    private async Task SendCommentAsync()
    {
        User.LeaveComment(CommentText, Profile);
        await RefreshComments();
        CommentText = string.Empty;
    }

    public async Task LoadAsync()
    {
        Nickname = Profile.Nickname;
        AboutSelf = Profile.About;

        var bitmap = await Profile.GetImageBitmapAsync();
        if (bitmap is { }) Avatar = bitmap;

        await RefreshComments();
    }

    private async Task RefreshComments()
    {
        Comments.Clear();
        var comments = await Profile.GetComments();
        foreach (var comment in comments) 
            Comments.Add(new CommentViewModel(comment, User));
    }
}