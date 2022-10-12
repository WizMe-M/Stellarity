using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.Models;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

[ObservableObject]
public partial class MyProfileViewModel : IAsyncImageLoader
{
    public MyProfileViewModel(AccountingService service)
    {
        User = service.AuthorizedAccount!;
        Avatar = ImagePlaceholder.GetBitmap();
    }

    public ObservableCollection<Comment> Comments { get; } = new();

    [ObservableProperty]
    private Account _user = null!;

    [ObservableProperty]
    private Bitmap? _avatar;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SendCommentCommand))]
    private string _commentText = string.Empty;

    public bool CanComment => !string.IsNullOrWhiteSpace(_commentText);

    [RelayCommand(CanExecute = nameof(CanComment))]
    private async Task SendCommentAsync()
    {
        User.LeaveComment(_commentText, User);
        var comments = await User.LoadCommentsFor(User);
        Comments.Clear();
        Comments.AddRange(comments);
    }

    public async Task LoadAsync()
    {
        var bitmap = await User.GetImageBitmapAsync();
        if (bitmap is { }) Avatar = bitmap;
    }
}