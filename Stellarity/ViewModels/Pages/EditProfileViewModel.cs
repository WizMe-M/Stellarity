using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stellarity.Basic;
using Stellarity.Database.Entities;
using Stellarity.Extensions;

namespace Stellarity.ViewModels.Pages;

public class EditProfileViewModel : ValidatableViewModelBase, IAsyncImageLoader
{
    /// <summary>
    /// Viewmodel to resolve view for <see cref="IDialogService"/>
    /// </summary>
    private readonly MainViewModel _windowOwner = null!;

    private readonly IDialogService _dialogService = null!;
    private readonly Account _user;

    public EditProfileViewModel(Account user)
    {
        _user = user;
        Nickname = _user.Nickname ?? _user.Email;
        AboutSelf = _user.About;

        SaveChanges = ReactiveCommand.CreateFromTask(async () =>
        {
            // Account.SaveChanges(user);
        });

        SetAvatar = ReactiveCommand.CreateFromTask(async () =>
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
        });
        ChangePassword = ReactiveCommand.Create(() => { });
    }


    public EditProfileViewModel(IDialogService dialogService, MainViewModel windowOwner, Account user) : this(user)
    {
        _dialogService = dialogService;
        _windowOwner = windowOwner;
    }

    [Reactive]
    public Bitmap? Avatar { get; private set; }

    [Reactive]
    public string Nickname { get; set; }

    [Reactive]
    public string? AboutSelf { get; set; }

    public ICommand SaveChanges { get; }
    public ICommand SetAvatar { get; }
    public ICommand ChangePassword { get; }

    public Task<Bitmap?> LoadAsync()
    {
        var bm = _user.Avatar?.Data.ToBitmap();
        Avatar = bm ?? Avatar;
        return Task.FromResult(bm);
    }
}