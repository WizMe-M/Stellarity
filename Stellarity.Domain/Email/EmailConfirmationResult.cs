using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Email;

public class EmailConfirmationResult : Result
{
    private EmailConfirmationResult(string? errorMessage = null)
        : base(errorMessage)
    {
        IsSuccessful = ErrorMessage is { };
    }

    public override bool IsSuccessful { get; }

    public static EmailConfirmationResult Success() => new();

    public static EmailConfirmationResult NotValidEmail()
        => new("Such email isn't valid");
}