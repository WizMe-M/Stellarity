using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Avalonia.ViewModel;

namespace Stellarity.Desktop.ViewModels;

public partial class ChangePasswordViewModel : ViewModelBase, IModalDialogViewModel, ICloseable
{
    public event EventHandler? RequestClose;
    public bool? DialogResult { get; private set; }

    private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+=])(?!.*\s).{8,}$";

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _newPassword = string.Empty;

    public ChangePasswordViewModel()
    {
        Validator = GetValidator();
    }

    [RelayCommand(CanExecute = nameof(IsPasswordCorrect))]
    public void Confirm()
    {
        DialogResult = true;
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogResult = false;
        Close();
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<ChangePasswordViewModel>();

        builder.RuleFor(vm => vm.NewPassword)
            .NotEmpty()
            .WithMessage("The password mustn't be empty string")
            .Matches(PasswordPattern)
            .WithMessage("The password must contain: 1 digit, 1 uppercase, 1 lowercase, 1 special symbol; " +
                         "and it's length should be at least 8 or more symbols")
            .MinLength(8)
            .WithMessage("The password must be at least 8 characters long");

        return builder.Build(this);
    }

    private bool IsPasswordCorrect => Validator!.IsValid;

    private void Close()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}