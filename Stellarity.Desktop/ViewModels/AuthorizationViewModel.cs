using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;

namespace Stellarity.Desktop.ViewModels;

public partial class AuthorizationViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService = null!;
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

    public AuthorizationViewModel(IDialogService dialogService, AccountingService accountingService) : this()
    {
        _dialogService = dialogService;
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
            await ShowErrorAsync(authorizationResult.ErrorMessage!, "Authorization error");
            return;
        }

        OpenMainView();
    }

    [RelayCommand]
    private void ShowChangePassword()
    {
        var changePasswordViewModel = _dialogService.CreateViewModel<ChangePasswordForEmailViewModel>();
        _dialogService.Show(this, changePasswordViewModel);
        _dialogService.Close(this);
    }

    [RelayCommand]
    private void ShowRegistration()
    {
        var registerPlayerViewModel = _dialogService.CreateViewModel<RegisterPlayerViewModel>();
        _dialogService.Show<RegisterPlayerView>(this, registerPlayerViewModel);
        _dialogService.Close(this);
    }

    public async Task TryAutoLogInAsync()
    {
        if (!_accountingService.HaveAuthHistory) return;

        if (_accountingService.IsAutoLogIn)
        {
            OpenMainView();
            return;
        }

        if (_accountingService.AuthorizedUser is { IsBanned: true })
        {
            await ShowErrorAsync("User was banned. Log into another account", "Authorization error");
        }
    }

    private Task ShowErrorAsync(string text, string title) =>
        _dialogService.ShowMessageBoxAsync(this, text, title, MessageBoxButton.Ok, MessageBoxImage.Error);

    private void OpenMainView()
    {
        var vm = _dialogService.CreateViewModel<MainViewModel>();
        _dialogService.Show(this, vm);
        _dialogService.Close(this);
    }
}