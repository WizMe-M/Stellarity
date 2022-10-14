﻿using System.Collections.ObjectModel;
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

    public ObservableCollection<Comment> Comments { get; } = new();

    public bool CanComment => !string.IsNullOrWhiteSpace(_commentText);

    [RelayCommand(CanExecute = nameof(CanComment))]
    private async Task SendCommentAsync()
    {
        User.LeaveComment(_commentText, User);
        await RefreshComments();
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
        var comments = await User.GetComments();
        Comments.Clear();
        Comments.AddRange(comments);
    }
}