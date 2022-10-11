namespace Stellarity.Domain.Authorization;

public class AuthorizationHistory
{
    public AuthorizationHistory(string userEmail, bool rememberLastAuthorizedUser)
    {
        RememberLastAuthorizedUser = rememberLastAuthorizedUser;
        UserEmail = userEmail;
    }

    public string UserEmail { get; }

    public bool RememberLastAuthorizedUser { get; }
}