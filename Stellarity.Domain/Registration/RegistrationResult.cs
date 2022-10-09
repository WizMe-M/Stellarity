using Stellarity.Domain.Models;

namespace Stellarity.Domain.Registration;

public class RegistrationResult
{
    private RegistrationResult(Account? acc, string message = "")
    {
        Account = acc;
        Message = message;
    }


    public Account? Account { get; }

    public string Message { get; }

    public bool IsSuccessful => Account is { };

    public static RegistrationResult Fail() => new(null,
        "User with such email already exists");

    public static RegistrationResult Success(Account account) => new(account);
}