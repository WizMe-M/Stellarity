using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;

namespace Stellarity.Desktop.ViewModels;

public partial class AuthorizationViewModel : ViewModelBase
{
    private readonly IDialogService _windowService = null!;
    private readonly AccountingService _accountingService = null!;

    [ObservableProperty] private string _email = string.Empty;

    [ObservableProperty] private string _password = string.Empty;

    [ObservableProperty] private bool _rememberMe;

    private AuthorizationViewModel()
    {
        Email = "";
        Password = "";
        RememberMe = true;
    }

    public AuthorizationViewModel(IDialogService windowService, AccountingService accountingService) : this()
    {
        _windowService = windowService;
        _accountingService = accountingService;
    }

    [RelayCommand]
    private async Task LogInAsync()
    {
        Email = Email.Trim();
        Password = Password.Trim();
        var hashedPassword = MD5Hasher.Encrypt(Password);

        var authorizationResult = await _accountingService.AccountAuthorizationAsync(Email, hashedPassword, RememberMe);
        if (!authorizationResult.IsSuccessful)
        {
            await ShowErrorAsync(authorizationResult.Message, "Authorization error");
            return;
        }

        OpenMainView();
    }

    public async Task TryAutoLogInAsync()
    {
        if (!_accountingService.HaveAuthHistory) return;

        if (_accountingService.IsAutoLogIn)
        {
            OpenMainView();
            return;
        }

        if (_accountingService.AuthorizedAccount!.IsBanned)
        {
            await ShowErrorAsync("User was banned. Log into another account", "Authorization error");
        }
    }

    private Task ShowErrorAsync(string text, string title) =>
        _windowService.ShowMessageBoxAsync(this, text, title, MessageBoxButton.Ok, MessageBoxImage.Error);

    private void OpenMainView()
    {
        var vm = _windowService.CreateViewModel<MainViewModel>();
        _windowService.Show(this, vm);
        _windowService.Close(this);
    }
}