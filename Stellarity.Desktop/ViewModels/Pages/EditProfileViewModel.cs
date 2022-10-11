using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Services.Accounting;
using Image = Stellarity.Avalonia.Models.Image;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class EditProfileViewModel : ViewModelBase, IAsyncImageLoader
{
    private byte[]? _previousAvatarData;
    private byte[]? _currentAvatarData;
    private string? _previousNickname;
    private string? _previousAbout;

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
    private readonly AccountEntity _user;

    private EditProfileViewModel(AccountEntity user)
    {
        _user = user;
        _previousAvatarData = Image.GetPlaceholderBytes();
        _currentAvatarData = _previousAvatarData;
        _previousNickname = _user.Nickname ?? string.Empty;
        _previousAbout = _user.About ?? string.Empty;

        Nickname ??= string.Empty;
        AboutSelf ??= string.Empty;
    }

    public EditProfileViewModel(IDialogService dialogService, AccountingService accounting, MainViewModel windowOwner) :
        this(accounting.AuthorizedAccount!)
    {
        _dialogService = dialogService;
        _windowOwner = windowOwner;
    }

    public Bitmap? Avatar
    {
        get => _currentAvatarData.ToBitmap();
        set
        {
            var data = value.FromBitmap();
            if (EqualityComparer<byte[]?>.Default.Equals(_currentAvatarData, data)) return;

            SetProperty(ref _currentAvatarData, data);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
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

    public bool HasChanges() => _currentAvatarData != null && _previousAvatarData != _currentAvatarData
                                || Nickname != _previousNickname || AboutSelf != _previousAbout;

    public async Task LoadAsync()
    {
        var bytes = await _user.GetAvatarAsync();
        var bm = bytes.ToBitmap();
        Avatar = bm ?? Avatar;
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

    [RelayCommand(CanExecute = nameof(HasChanges))]
    private async Task SaveChangesAsync()
    {
        await _user.SaveChangesAsync();
        await _user.ChangeAvatarAsync(_currentAvatarData);

        _previousAvatarData = _currentAvatarData;
        _previousNickname = Nickname;
        _previousAbout = AboutSelf;
    }
}