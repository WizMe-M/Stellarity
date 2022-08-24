using Stellarity.Database.Entities;

namespace Stellarity.Services.Accounting;

public class AuthorizationInfo
{
    public AuthorizationInfo(Account lastAuthorizedUser, bool rememberLastAuthorizedUser)
    {
        LastAuthorizedUser = lastAuthorizedUser;
        RememberLastAuthorizedUser = rememberLastAuthorizedUser;
    }

    public Account? LastAuthorizedUser { get; }
    public bool RememberLastAuthorizedUser { get; }
}