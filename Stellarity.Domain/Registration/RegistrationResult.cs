using Stellarity.Domain.Models;

namespace Stellarity.Domain.Registration;

public class RegistrationResult
{
    private RegistrationResult(Account? acc = null, string errorMessage = "")
    {
        Account = acc;
        ErrorMessage = errorMessage;
    }

    public Account? Account { get; }

    public string ErrorMessage { get; }

    public bool IsSuccessful => Account is { };

    public static RegistrationResult AlreadyExistsWithEmail() => new(errorMessage: "User with such email already exists");

    public static RegistrationResult Success(Account account) => new(account);
}