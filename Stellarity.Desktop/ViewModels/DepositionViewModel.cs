using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;

namespace Stellarity.Desktop.ViewModels;

public partial class DepositionViewModel : ViewModelBase, IModalDialogViewModel, ICloseable
{
    [ObservableProperty]
    private decimal _depositionSum;

    public event EventHandler? RequestClose;

    public DepositionViewModel()
    {
        DepositionSum = 100;
        Validator = GetValidator();
    }

    public bool? DialogResult { get; set; }

    public bool CanConfirm => Validator is { IsValid: true };

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        DialogResult = true;
        Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        DialogResult = false;
        Close();
    }

    private void Close() => RequestClose?.Invoke(this, EventArgs.Empty);

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<DepositionViewModel>();

        builder.RuleFor(vm => vm.DepositionSum)
            .Between(100, 125000)
            .WithMessage("Amount of deposition can't be less than 100 or more than 125 000");

        return builder.Build(this);
    }
}