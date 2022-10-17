using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;
using Stellarity.Domain.Validation;

namespace Stellarity.Desktop.ViewModels;

public partial class RegisterPlayerViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _email = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = string.Empty;

    public RegisterPlayerViewModel()
    {
        _dialogService = DiContainingService.Kernel.Get<IDialogService>();
        Validator = GetValidator();
    }

    public bool CanRegister => Validator is { IsValid: true };

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<RegisterPlayerViewModel>();

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

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        var registrationResult = await accountingService.AccountPlayerRegistrationAsync(Email, Password);
        Email = string.Empty;
        Password = string.Empty;
        if (registrationResult.IsSuccessful)
        {
            var mainViewModel = _dialogService.CreateViewModel<MainViewModel>();
            _dialogService.Show<MainView>(this, mainViewModel);
            _dialogService.Close(this);
            return;
        }

        await _dialogService.ShowMessageBoxAsync(this, registrationResult.ErrorMessage,
            "Ошибка регистрации", MessageBoxButton.Ok, MessageBoxImage.Error);
    }

    [RelayCommand]
    private void ShowAuthorization()
    {
        var authorizationViewModel = _dialogService.CreateViewModel<AuthorizationViewModel>();
        _dialogService.Show<AuthorizationView>(this, authorizationViewModel);
        _dialogService.Close(this);
    }
}