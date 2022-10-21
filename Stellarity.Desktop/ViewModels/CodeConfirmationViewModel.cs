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

    [ObservableProperty] private string _input = string.Empty;

    private int _code;
    private string _email = null!;

    private Func<string, string, Task<EmailDeliverResult>> _sendConfirmation = null!;

    public CodeConfirmationViewModel()
    {
        Validator = GetValidator();
        SendConfirmationCodeAsync().Wait();
    }

    public bool? DialogResult { get; private set; }

    public bool CanConfirm => Validator is { IsValid: true };

    public void InitializeMailing(string email, EmailTypes type)
    {
        _email = email;
        var mailing = DiContainingService.Kernel.Get<MailingService>();
        _sendConfirmation = type switch
        {
            EmailTypes.ActivateUser => (mail, code) => mailing.SendConfirmAccount(mail, code),
            EmailTypes.ConfirmPasswordChange => (mail, code) => mailing.SendChangePassword(mail, code),
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
        await _sendConfirmation(_email, _code.ToString());
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