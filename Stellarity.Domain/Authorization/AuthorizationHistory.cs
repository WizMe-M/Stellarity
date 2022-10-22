namespace Stellarity.Domain.Authorization;

public record AuthorizationHistory(string UserEmail, bool RememberLastAuthorizedUser);