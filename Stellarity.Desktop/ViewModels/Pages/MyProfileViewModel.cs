using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stellarity.Avalonia.Extensions;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Services.Accounting;
using Image = Stellarity.Avalonia.Models.Image;

namespace Stellarity.Desktop.ViewModels.Pages;

[ObservableObject]
public partial class MyProfileViewModel : IAsyncImageLoader
{
    public MyProfileViewModel(AccountingService service)
    {
        User = service.AuthorizedAccount!;
        Avatar = Image.GetPlaceholderBitmap();
    }

    public ObservableCollection<CommentEntity> Comments { get; } = new();

    [ObservableProperty] private AccountEntity _user = null!;

    [ObservableProperty] private Bitmap? _avatar;

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
        var comment = CommentEntity.Add(_commentText, User);
        Comments.Add(comment);
    }

    public async Task LoadAsync()
    {
        var bytes = await User.GetAvatarAsync();
        var bm = bytes.ToBitmap();
        Avatar = bm ?? Avatar;
    }
}