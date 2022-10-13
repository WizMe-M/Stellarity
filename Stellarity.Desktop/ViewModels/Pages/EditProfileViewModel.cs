using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class EditProfileViewModel : ViewModelBase, IAsyncImageLoader
{
    private byte[]? _previousAvatarData;
    private byte[]? _currentAvatarData;
    private string? _previousNickname;
    private string? _previousAbout;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _currentNickname = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _currentAbout = string.Empty;

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

    private EditProfileViewModel(Account user)
    {
        _user = user;
        _previousAvatarData = _user.TryGetImageBytes();
        _currentAvatarData = _previousAvatarData;
        _previousAbout = _user.About;
        _previousNickname = _user.Nickname;

        if (_user.IsNicknameSet) CurrentNickname = _previousNickname;
        CurrentAbout = _previousAbout;
    }

    public EditProfileViewModel(IDialogService dialogService, AccountingService accounting, MainViewModel windowOwner)
        : this(accounting.AuthorizedAccount!)
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

    private bool ImageWasChanged => _previousAvatarData != _currentAvatarData;
    private bool ProfileInfoWasChanged => CurrentNickname != _previousNickname || CurrentAbout != _previousAbout;

    public bool HasChanges => ImageWasChanged || ProfileInfoWasChanged;

    public async Task LoadAsync()
    {
        var bytes = await _user.GetImageBytesAsync();
        var bitmap = bytes.ToBitmap();
        if (bitmap is { })
        {
            Avatar = bitmap;
            _previousAvatarData = bytes;
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

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        var vm = _dialogService.CreateViewModel<ChangePasswordViewModel>();
        var passwordChanged = await _dialogService.ShowDialogAsync(_windowOwner, vm);
        if (passwordChanged == true)
        {
            var password = vm.NewPassword!;
            _user.ApplySatisfiedPassword(password);
        }
    }

    [RelayCommand(CanExecute = nameof(HasChanges))]
    private async Task SaveChangesAsync()
    {
        if (ProfileInfoWasChanged) await _user.EditProfileInfoAsync(CurrentNickname, CurrentAbout);
        if (ImageWasChanged) await _user.SetImageAsync(_currentAvatarData!, _user.Email);

        _previousAvatarData = _currentAvatarData;
        _previousNickname = CurrentNickname;
        _previousAbout = CurrentAbout;
    }
}