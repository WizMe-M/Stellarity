using Stellarity.Database.Entities;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services.Cache;

namespace Stellarity.Domain.Authorization;

public class AccountingService : CachingBaseService<AuthorizationHistory>
{
    private AuthorizationHistory? _authorizationHistory;
    private readonly string _cacheFileName;

    public AccountingService(Cacher cacher, string cacheFileName = "auth.info")
        : base(cacher, "Accounting/", CachingType.Json)
    {
        _cacheFileName = cacheFileName;
    }

    public Account? AuthorizedUser { get; private set; }

    public bool IsAutoLogIn => _authorizationHistory is { RememberLastAuthorizedUser: true }
                               && AuthorizedUser is { IsBanned: false };

    public bool HaveAuthHistory => _authorizationHistory is { };

    /// <summary>
    /// Asynchronously initializes accounting service with cached data
    /// </summary>
    public async Task InitializeAsync()
    {
        _authorizationHistory = await LoadAuthorizationHistoryAsync();
        if (_authorizationHistory is { RememberLastAuthorizedUser: true })
            AutoLogIn();
    }

    /// <summary>
    /// Automatically authorize user
    /// </summary>
    private void AutoLogIn()
    {
        var accountEntity = AccountEntity.Find(_authorizationHistory!.UserEmail);
        if (accountEntity is { Deleted: false })
            AuthorizedUser = new Account(accountEntity);
    }

    public async Task<AuthorizationResult> AccountAuthorizationAsync(string email, string password, bool remember)
    {
        var authResult = await Account.AuthorizeAsync(email, password);
        if (!authResult.IsSuccessful) return authResult;

        AuthorizedUser = authResult.Account;
        await SaveAuthorizationHistoryAsync(authResult.Account!.Email, remember);
        return authResult;
    }

    private Task<AuthorizationHistory?> LoadAuthorizationHistoryAsync() => LoadAsync(_cacheFileName);

    private async Task SaveAuthorizationHistoryAsync(string authorizedUserEmail, bool remember)
    {
        var authorizationHistory = new AuthorizationHistory(authorizedUserEmail, remember);
        await SaveAsync(_cacheFileName, authorizationHistory);
    }
}