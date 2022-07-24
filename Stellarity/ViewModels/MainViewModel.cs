using System.Diagnostics;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Basic;

namespace Stellarity.ViewModels;

public class MainViewModel : ValidatableViewModelBase
{
    /// <summary>
    /// Password must contain at least: one letter in lowercase, one letter in uppercase,
    /// one digit, one special symbol and it's length should be between 8 and 12.
    /// </summary>
    private const string PasswordPattern = "^(?=.*[a-z])" +
                                           "(?=.*[A-Z])" +
                                           "(?=.*[0-9])" +
                                           "(?=.*[\\-\\\\/\\.,<>?'\"`:;|~!@#$%^&*()+=№])" +
                                           "([a-zA-Z0-9\\-\\\\/\\.,<>?\'\"`:;|~!@#$%^&*()+=№]{8,})$";

    public MainViewModel()
    {
        Validator = GetValidator();
        var canRegister = this.WhenAnyValue(model => model.Validator!.IsValid);
        Register = ReactiveCommand.Create(() => { Debug.WriteLine($"email: {Email}; password: {Password}"); },
            canRegister);

        HaveAccount = ReactiveCommand.Create(() => { Debug.WriteLine($"email: {Email}; password: {Password}"); });
        Email = "E@mail.ru";
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<MainViewModel>();

        builder.RuleFor(vm => vm.Email)
            .NotEmpty();

        builder.RuleFor(vm => vm.Password)
            .NotEmpty()
            .Matches(PasswordPattern)
            .WithMessage("Your password too weak!");

        return builder.Build(this);
    }

    [Reactive]
    public string Email { get; set; }

    [Reactive]
    public string Password { get; set; }

    public ICommand Register { get; }

    public ICommand HaveAccount { get; }
}