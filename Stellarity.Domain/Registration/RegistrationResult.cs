using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Registration;

public class RegistrationResult : Result
{
    private RegistrationResult(Account? acc = null, string errorMessage = "")
        : base(errorMessage)

    {
        Account = acc;
    }

    public Account? Account { get; }

    public override bool IsSuccessful => Account is { };

    public static RegistrationResult AlreadyExistsWithEmail() =>
        new(errorMessage: "User with such email already exists");

    public static RegistrationResult Success(Account account) => new(account);
}