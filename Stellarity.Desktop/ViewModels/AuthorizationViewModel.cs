using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Email;
using Stellarity.Domain.Models;

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

            if (authorizationResult.ErrorCode is not (int)AuthErrorCodes.UserNotActivated) return;

            var codeConfirmationViewModel = _dialogService.CreateViewModel<CodeConfirmationViewModel>();
            codeConfirmationViewModel.InitializeMailing(Email, EmailType.AccountActivation);

            var isConfirmed =
                await _dialogService.ShowDialogAsync<CodeConfirmationView>(this, codeConfirmationViewModel);

            switch (isConfirmed)
            {
                case null:
                case false:
                    await _dialogService.ShowMessageBoxAsync(this,
                        "Sorry, activation process wasn't finished.\n" +
                        "If you doesn't see code in inbox, look into folder spam or check was your email inputted correctly",
                        "Error", MessageBoxButton.Ok, MessageBoxImage.Error);
                    break;
                case true:
                    Account.Activate(_email);
                    break;
            }

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

        if (_accountingService.AuthorizedUser is { IsActivated: false })
        {
            await ShowErrorAsync("User isn't activated", "Authorization error");
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