using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Email;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels;

public partial class CodeConfirmationViewModel : ViewModelBase, IModalDialogViewModel, ICloseable
{
    public event EventHandler? RequestClose;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _input = string.Empty;

    private int _code;
    private string _email = null!;

    private Func<string, int, Task<EmailDeliverResult>> _sendConfirmation = null!;

    public CodeConfirmationViewModel()
    {
        Validator = GetValidator();
    }

    public bool? DialogResult { get; private set; }

    public bool CanConfirm => Validator is { IsValid: true };

    public void InitializeMailing(string email, EmailType type)
    {
        _email = email;
        var mailing = DiContainingService.Kernel.Get<MailingService>();
        _sendConfirmation = type switch
        {
            EmailType.AccountActivation => (mail, code) => mailing.SendAccountActivationCodeAsync(mail, code),
            EmailType.PasswordChange => (mail, code) => mailing.SendChangePasswordConfirmationCodeAsync(mail, code),
            EmailType.PurchaseCheque => throw new NotSupportedException(),
            _ => throw new NotSupportedException()
        };
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<CodeConfirmationViewModel>();
        builder.RuleFor(vm => vm.Input).Length(6);
        return builder.Build(this);
    }

    private void Close()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void GenerateCode()
    {
        _code = new Random().Next(100000, 999999);
    }

    [RelayCommand]
    private async Task SendConfirmationCodeAsync()
    {
        GenerateCode();
        await _sendConfirmation(_email, _code);
    }

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        if (_code.ToString() != Input)
        {
            GenerateCode();
            Input = string.Empty;
            DialogResult = false;
            return;
        }

        DialogResult = true;
        Close();
    }
}