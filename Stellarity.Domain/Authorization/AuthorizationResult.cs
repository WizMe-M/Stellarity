using Stellarity.Domain.Models;

namespace Stellarity.Domain.Authorization;

public class AuthorizationResult
{
    private AuthorizationResult(Account? account = null, string message = "")
    {
        Account = account;
        Message = message;
    }

    public Account? Account { get; }

    public string Message { get; }

    public bool IsSuccessful => Account is { };

    public static AuthorizationResult NoSuchUser() =>
        new(message: "User with such email and/or password doesn't exist");

    public static AuthorizationResult UserWasBanned() => new(message: "This user was banned by administrator");

    public static AuthorizationResult Success(Account account) => new(account);
}