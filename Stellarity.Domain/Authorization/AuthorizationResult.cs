using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Authorization;

public class AuthorizationResult : Result
{
    private AuthorizationResult(Account? account = null, string? errorMessage = null)
        : base(errorMessage)
    {
        Account = account;
    }

    public Account? Account { get; }

    public override bool IsSuccessful => Account is { };

    public static AuthorizationResult NoSuchUser() =>
        new(errorMessage: "User with such email and/or password doesn't exist");

    public static AuthorizationResult UserWasBanned() => new(errorMessage: "This user was banned by administrator");

    public static AuthorizationResult Success(Account account) => new(account);
}