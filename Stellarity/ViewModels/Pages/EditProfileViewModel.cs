﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.Pages;

[ObservableObject]
public partial class EditProfileViewModel : IAsyncImageLoader
{
    /// <summary>
    /// Viewmodel to resolve view for <see cref="IDialogService"/>
    /// </summary>
    private readonly MainViewModel _windowOwner = null!;

    /// <summary>
    /// Service to show dialog windows
    /// </summary>
    private readonly IDialogService _dialogService = null!;

    /// <summary>
    /// Current authorized user
    /// </summary>
    private readonly Account _user;

    [ObservableProperty]
    private Bitmap? _avatar;

    [ObservableProperty]
    private string _nickname = string.Empty;

    [ObservableProperty]
    private string? _aboutSelf;

    public EditProfileViewModel(Account user)
    {
        _user = user;
        Nickname = _user.Nickname ?? _user.Email;
        AboutSelf = _user.About;
    }

    public EditProfileViewModel(IDialogService dialogService, MainViewModel windowOwner, Account user) : this(user)
    {
        _dialogService = dialogService;
        _windowOwner = windowOwner;
    }

    [RelayCommand]
    public void SaveChanges()
    {
    }

    [RelayCommand]
    public async Task ChangePassword()
    {
    }

    [RelayCommand]
    public async Task SetAvatar()
    {
        var settings = new OpenFileDialogSettings
        {
            Title = "Выберите изображение для аватара своего профиля",
            InitialDirectory = Directory.GetDirectoryRoot(Assembly.GetExecutingAssembly().Location),
            Filters = new List<FileFilter>(new[]
            {
                new FileFilter("Изображения", new[] { "jpg", "jpeg", "png" })
            })
        };

        var imgPath = await _dialogService.ShowOpenFileDialogAsync(_windowOwner, settings);
        if (imgPath is { })
        {
            Avatar = new Bitmap(imgPath);
        }
    }

    public Task<Bitmap?> LoadAsync()
    {
        var bm = _user.Avatar?.Data.ToBitmap();
        Avatar = bm ?? Avatar;
        return Task.FromResult(bm);
    }
}