using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Email;
using Stellarity.Domain.Models;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Services;
using Stellarity.Domain.Validation;

namespace Stellarity.Desktop.ViewModels;

public partial class ChangePasswordForEmailViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly AccountingService _accountingService;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _password = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _email = string.Empty;

    public ChangePasswordForEmailViewModel()
    {
        _dialogService = DiContainingService.Kernel.Get<IDialogService>();
        _accountingService = DiContainingService.Kernel.Get<AccountingService>();
        Validator = GetValidator();
    }

    public bool CanChangePassword => Validator is { IsValid: true };

    [RelayCommand(CanExecute = nameof(CanChangePassword))]
    private async Task Confirm()
    {
        var exists = AccountEntity.Exists(Email);
        if (!exists)
        {
            Email = string.Empty;
            Password = string.Empty;
            await _dialogService.ShowMessageBoxAsync(this, "User with such email doesn't exist",
                "Error", MessageBoxButton.Ok, MessageBoxImage.Error);
            return;
        }

        var confirmationViewModel = _dialogService.CreateViewModel<CodeConfirmationViewModel>();
        confirmationViewModel.InitializeMailing(Email, EmailType.PasswordChange);
        var confirmationResult = await _dialogService.ShowDialogAsync(this, confirmationViewModel);
        if (confirmationResult is not true)
        {
            Password = string.Empty;
            await _dialogService.ShowMessageBoxAsync(this,
                "Sorry, we can't change password without email confirmation." +
                "\nIf you doesn't see code in inbox, see folders spam or check your email was inputted correctly",
                "Error", MessageBoxButton.Ok, MessageBoxImage.Warning);
            return;
        }

        var hashed = Account.ChangePassword(Email, Password);

        var authResult = await _accountingService.AccountAuthorizationAsync(Email, hashed.Password, false);
        if (!authResult.IsSuccessful)
        {
            await _dialogService.ShowMessageBoxAsync(this, "Oops, something went wrong...", "Error",
                MessageBoxButton.Ok, MessageBoxImage.Error);
            var authorizationViewModel = _dialogService.CreateViewModel<AuthorizationViewModel>();
            _dialogService.Show<AuthorizationView>(this, authorizationViewModel);
            _dialogService.Close(this);
            return;
        }

        var mainViewModel = _dialogService.CreateViewModel<MainViewModel>();
        _dialogService.Show<MainView>(this, mainViewModel);
        _dialogService.Close(this);
    }

    [RelayCommand]
    private void Cancel()
    {
        var authorizationViewModel = _dialogService.CreateViewModel<AuthorizationViewModel>();
        _dialogService.Show<AuthorizationView>(this, authorizationViewModel);
        _dialogService.Close(this);
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<ChangePasswordForEmailViewModel>();

        builder.RuleFor(vm => vm.Email)
            .NotEmpty()
            .WithMessage("Email mustn't be empty string")
            .MaxLength(320)
            .WithMessage("Email too long")
            .Must(email => UserValidation.IsRealEmail(email))
            .WithMessage("This email isn't valid. Correct format: {local}@{domain}");

        builder.RuleFor(vm => vm.Password)
            .NotEmpty()
            .WithMessage("The password mustn't be empty string")
            .Must(password => UserValidation.IsCorrectPassword(password))
            .WithMessage("The password must contain: 1 digit, 1 uppercase, 1 lowercase, 1 special symbol; " +
                         "and it's length should be at least 8 or more symbols")
            .MinLength(8)
            .WithMessage("The password must be at least 8 characters long")
            .MaxLength(12)
            .WithMessage("The password must be less or equal than 12 characters long");

        return builder.Build(this);
    }
}