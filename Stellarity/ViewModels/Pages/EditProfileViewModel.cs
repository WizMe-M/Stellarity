using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
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

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private Bitmap? _avatar;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _nickname = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
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

    public string Password
    {
        get
        {
            var pattern = new Regex("."); // matches any symbol
            return pattern.Replace(_user.Password, "*");
        }
    }

    public bool HasChanges() => Avatar != null && Avatar != _user.Avatar?.Data.ToBitmap()
                                || !string.IsNullOrWhiteSpace(_nickname) && Nickname != _user.Nickname
                                || AboutSelf != _user.About;

    [RelayCommand(CanExecute = nameof(HasChanges))]
    public async Task SaveChangesAsync()
    {
        _user.Nickname = _nickname;
        _user.About = AboutSelf;
        Avatar = null;
        await Task.Delay(2000);
        // TODO: apply changes to user and to image
        // await Image.SaveAsync();
        // await _user.SaveChangesAsync();
    }

    [RelayCommand]
    public async Task ChangePasswordAsync()
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