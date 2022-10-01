using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;
using Stellarity.Services.Accounting;

namespace Stellarity.ViewModels.Pages;

[ObservableObject]
public partial class MyProfileViewModel : IAsyncImageLoader
{
    public MyProfileViewModel(AccountingService service)
    {
        User = service.AuthorizedAccount!;
        Avatar = Image.OpenDefaultImage();
    }

    public ObservableCollection<Comment> Comments { get; } = new();

    [ObservableProperty]
    private Account _user;

    [ObservableProperty]
    private Bitmap? _avatar;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SendCommentCommand))]
    private string _commentText = string.Empty;

    public bool CanComment => !string.IsNullOrWhiteSpace(_commentText);

    [RelayCommand]
    private void GoEditProfile()
    {
    }

    [RelayCommand(CanExecute = nameof(CanComment))]
    private void SendComment()
    {
        var comment = Comment.Send(_commentText, User);
        Comments.Add(comment);
    }

    public async Task LoadAsync()
    {
        var bytes = await User.GetAvatarAsync();
        var bm = bytes.ToBitmap();
        Avatar = bm ?? Avatar;
    }
}