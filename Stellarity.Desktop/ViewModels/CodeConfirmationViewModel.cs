using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Email;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels;

public partial class CodeConfirmationViewModel : ViewModelBase, IModalDialogViewModel, ICloseable
{
    public event EventHandler? RequestClose;

    [ObservableProperty] private string _input = string.Empty;

    private int _code;
    private string _email = null!;
    private readonly MailingService _mailing;

    public CodeConfirmationViewModel()
    {
        Validator = GetValidator();
        _mailing = DiContainingService.Kernel.Get<MailingService>();
        SendConfirmationCodeAsync().Wait();
    }


    public bool? DialogResult { get; private set; }

    public bool CanConfirm => Validator is { IsValid: true };

    public void InitializeEmail(string email) => _email = email;

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
        await _mailing.SendChangePassword(_email, _code.ToString());
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