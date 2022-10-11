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

    public Account? AuthorizedAccount { get; private set; }

    public bool IsAutoLogIn => _authorizationHistory is { RememberLastAuthorizedUser: true }
                               && AuthorizedAccount is { IsBanned: false };

    public bool HaveAuthHistory => _authorizationHistory is { };

    /// <summary>
    /// Asynchronously initializes accounting service with cached data
    /// </summary>
    public async Task InitializeAsync()
    {
        _authorizationHistory = await LoadAuthorizationHistoryAsync();
        if (_authorizationHistory is null) return;
        Initialize(_authorizationHistory);
    }

    /// <summary>
    /// Initialize accounting service with authorization info
    /// </summary>
    /// <param name="history">Cached info about authorization</param>
    private void Initialize(AuthorizationHistory history)
    {
        var accountEntity = AccountEntity.Find(history.UserEmail);
        if (accountEntity is null) return;
        AuthorizedAccount = new Account(accountEntity);
    }

    public async Task<AuthorizationResult> AccountAuthorizationAsync(string email, string password, bool remember)
    {
        var authResult = await Account.AuthorizeAsync(email, password);
        if (!authResult.IsSuccessful) return authResult;

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