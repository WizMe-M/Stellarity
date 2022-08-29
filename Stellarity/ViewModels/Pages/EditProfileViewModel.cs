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
    private string? _currentNickname;
    private string? _currentAbout;

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

    public EditProfileViewModel(Account user)
    {
        _user = user;
        _currentNickname = _user.Nickname ?? string.Empty;
        _currentAbout = _user.About ?? string.Empty;
        
        Nickname ??= string.Empty;
        AboutSelf ??= string.Empty;
    }

    public EditProfileViewModel(IDialogService dialogService, MainViewModel windowOwner, Account user) : this(user)
    {
        _dialogService = dialogService;
        _windowOwner = windowOwner;
    }

    public string? Nickname
    {
        get => _user.Nickname;
        set
        {
            if (EqualityComparer<string?>.Default.Equals(_user.Nickname, value)) return;

            SetProperty(_user.Nickname, value, _user,
                (account, nickname) => account.Nickname = nickname);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public string? AboutSelf
    {
        get => _user.About;
        set
        {
            if (EqualityComparer<string?>.Default.Equals(_user.About, value)) return;

            SetProperty(_user.About, value, _user,
                (account, about) => account.About = about);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
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
                                || Nickname != _currentNickname
                                || AboutSelf != _currentAbout;

    [RelayCommand(CanExecute = nameof(HasChanges))]
    private async Task SaveChangesAsync()
    {
        await _user.SaveChangesAsync();
        
        // TODO: apply changed avatar to user
        // TODO: make _currentAvatar field to handle changes
        await _user.ChangeAvatarAsync(Avatar!);
        
        await Task.Delay(2000);
        _currentNickname = Nickname;
        _currentAbout = AboutSelf;
        Avatar = null;
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        var vm = _dialogService.CreateViewModel<ChangePasswordViewModel>();
        var passwordChanged = await _dialogService.ShowDialogAsync(_windowOwner, vm);
        if (passwordChanged == true)
        {
            // _user.ChangePassword(vm.NewPassword);
        }
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