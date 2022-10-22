using Stellarity.Domain.Abstractions;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Authorization;

public class AuthorizationResult : Result
{
    private AuthorizationResult(Account? account = null, string? errorMessage = null, AuthErrorCodes? errorCode = null)
        : base(errorMessage, (int?)errorCode)
    {
        Account = account;
    }

    public Account? Account { get; }

    public override bool IsSuccessful => Account is { };

    public static AuthorizationResult NoSuchUser() =>
        new(errorCode: AuthErrorCodes.NoSuchUser, errorMessage: "User with such email and/or password doesn't exist");

    public static AuthorizationResult UserIsNotActivated() =>
        new(errorCode: AuthErrorCodes.UserNotActivated, errorMessage: "This user wasn't activated yet");

    public static AuthorizationResult UserWasBanned() =>
        new(errorCode: AuthErrorCodes.UserWasBanned, errorMessage: "This user was banned by administrator");

    public static AuthorizationResult Success(Account account) => new(account);
}