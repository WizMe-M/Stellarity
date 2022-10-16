using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Validation;

namespace Stellarity.Desktop.ViewModels;

public partial class ChangePasswordViewModel : ViewModelBase, IModalDialogViewModel, ICloseable
{
    public event EventHandler? RequestClose;
    public bool? DialogResult { get; private set; }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _input = string.Empty;

    public ChangePasswordViewModel()
    {
        Validator = GetValidator();
    }

    private bool IsPasswordCorrect => Validator!.IsValid;

    public HashedPassword? NewPassword { get; private set; }

    [RelayCommand(CanExecute = nameof(IsPasswordCorrect))]
    public void Confirm()
    {
        NewPassword = HashedPassword.FromDecrypted(_input);
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

        builder.RuleFor(vm => vm.Input)
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


    private void Close()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}