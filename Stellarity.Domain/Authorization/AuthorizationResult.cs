using Stellarity.Domain.Models;

namespace Stellarity.Domain.Authorization;

public class AuthorizationResult
{
    private AuthorizationResult(Account? account, string message = "")
    {
        Account = account;
        Message = message;
    }

    public Account? Account { get; }

    public string Message { get; }

    public bool IsSuccessful => Account is { };

    public static AuthorizationResult Fail() => new(null,
        "User with such email and/or password doesn't exist");

    public static AuthorizationResult FailBanned() => new(null, "This user was banned");

    public static AuthorizationResult Success(Account account) => new(account);
}