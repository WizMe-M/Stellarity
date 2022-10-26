using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Database;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Domain.Models;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Validation;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class RegisterUserViewModel : ViewModelBase
{
    private readonly NavigationPublisher _navigator;
    private readonly IDialogService _dialogService;

    [ObservableProperty] private Roles _selectedRole;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _email = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = string.Empty;

    public RegisterUserViewModel(NavigationPublisher navigator, IDialogService dialogService)
    {
        _navigator = navigator;
        _dialogService = dialogService;
        AvailableRoles.AddRange(Enum.GetValues<Roles>());
        SelectedRole = Roles.Player;

        Validator = GetValidator();
    }

    public ObservableCollection<Roles> AvailableRoles { get; } = new();

    public bool CanRegister => Validator is { IsValid: true };

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<RegisterUserViewModel>();

        builder.RuleFor(vm => vm.Email)
            .NotEmpty()
            .WithMessage("Email mustn't be empty string")
            .MaxLength(320)
            .WithMessage("Email too long")
            .Must(async (email, token) => await UserValidation.NotExistsWithEmailAsync(email, token))
            .WithMessage("User with such email already exists")
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

        builder.RuleFor(vm => vm.SelectedRole)
            .NotNull()
            .WithMessage("You must select role for user");

        return builder.Build(this);
    }

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        var registrationResult = await Account.RegisterUserAsync(Email, Password, SelectedRole, true);
        if (registrationResult.IsSuccessful) NavigateToCommunity();
        else
        {
            var mainViewModel = DiContainingService.Kernel.Get<MainViewModel>();
            await _dialogService.ShowMessageBoxAsync(mainViewModel, registrationResult.ErrorMessage!,
                "Error", MessageBoxButton.Ok, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateToCommunity();
    }

    private void NavigateToCommunity()
    {
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Pop());
    }
}