using Stellarity.Database.Entities;

namespace Stellarity.Services.Accounting;

public class AuthorizationInfo
{
    public AuthorizationInfo(AccountEntity lastAuthorizedUser, bool rememberLastAuthorizedUser)
    {
        LastAuthorizedUser = lastAuthorizedUser;
        RememberLastAuthorizedUser = rememberLastAuthorizedUser;
    }

    public AccountEntity? LastAuthorizedUser { get; }
    public bool RememberLastAuthorizedUser { get; }
}