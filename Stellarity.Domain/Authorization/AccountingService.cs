using Stellarity.Database.Entities;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services.Cache;

namespace Stellarity.Domain.Authorization;

public class AccountingService : CachingBaseService<AuthorizationHistory>
{
    private readonly string _cacheFileName;

    public AccountingService(Cacher cacher, string cacheFileName = "auth.info")
        : base(cacher, "Accounting/", CachingType.Json)
    {
        _cacheFileName = cacheFileName;
    }

    public bool IsUserRemembered { get; private set; }

    public Account AuthorizedAccount { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes accounting service with cached data
    /// </summary>
    public async Task InitializeAsync()
    {
        var info = await LoadAuthorizationHistoryAsync();
        if (info is null) return;
        Initialize(info);
    }

    /// <summary>
    /// Initialize accounting service with authorization info
    /// </summary>
    /// <param name="info">Cached info about authorization</param>
    private void Initialize(AuthorizationHistory info)
    {
        var accountEntity = AccountEntity.Find(info.UserEmail);
        AuthorizedAccount = new Account(accountEntity!);
        IsUserRemembered = info.RememberLastAuthorizedUser;
    }

    public async Task<AuthorizationResult> AuthorizeAsync(string email, string password, bool remember)
    {
        var authResult = await Account.AuthorizeAsync(email, password);
        if (!authResult.IsSuccessful) return authResult;

        AuthorizedAccount = authResult.Account!;
        IsUserRemembered = remember;
        SaveAuthorizationHistoryAsync(AuthorizedAccount.Email, remember);
        return authResult;
    }

    private async Task<AuthorizationHistory?> LoadAuthorizationHistoryAsync() => await LoadAsync(_cacheFileName);

    private async void SaveAuthorizationHistoryAsync(string authorizedUserEmail, bool remember)
    {
        var accountInfo = new AuthorizationHistory(authorizedUserEmail, remember);
        await SaveAsync(_cacheFileName, accountInfo);
    }
}