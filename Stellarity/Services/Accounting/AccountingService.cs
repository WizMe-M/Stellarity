using System;
using System.Threading.Tasks;
using Stellarity.Database.Entities;

namespace Stellarity.Services.Accounting;

public class AccountingService
{
    private readonly string _cacheFileName;
    private readonly CachingService _cacher;

    public AccountingService(CachingService cacher, string cacheFileName = "auth.info")
    {
        _cacher = cacher;
        _cacheFileName = cacheFileName;
    }

    public bool UserRemembered { get; private set; }
    public Account? AuthorizedAccount { get; private set; }

    /// <summary>
    /// Asynchronously initializes accounting service with cached data
    /// </summary>
    public async Task InitializeAsync()
    {
        var info = await LoadAuthorizationInfoAsync();
        try
        {
            Initialize(info);
        }
        catch (InvalidOperationException)
        {
            UserRemembered = false;
            AuthorizedAccount = null;
            ClearAuthorizationCache();
        }
    }

    /// <summary>
    /// Initialize accounting service with authorization info
    /// </summary>
    /// <param name="info">Cached info about authorization</param>
    /// <exception cref="InvalidOperationException">User remembered, but no user was cached</exception>
    private void Initialize(AuthorizationInfo? info)
    {
        if (info is null) return;
        if (info.RememberLastAuthorizedUser && info.LastAuthorizedUser is null)
            throw new InvalidOperationException("Missing remembered user");

        AuthorizedAccount = info.LastAuthorizedUser;
        UserRemembered = info.RememberLastAuthorizedUser;
    }

    public Task<bool> AuthorizeAsync(string email, string password, bool remember)
    {
        // TODO: find user by email and password
        var authenticated = email == "test@mail.ru" && password == "password";
        if (authenticated)
        {
            AuthorizedAccount = new Account(email, password);
            UserRemembered = remember;
            SaveAuthorizationInfoAsync();
        }

        return Task.FromResult(authenticated);
    }

    private async Task<AuthorizationInfo?> LoadAuthorizationInfoAsync()
    {
        var accountInfo = await _cacher.LoadFromJsonCacheAsync<AuthorizationInfo>(_cacheFileName);
        return accountInfo;
    }

    private async void SaveAuthorizationInfoAsync()
    {
        if (AuthorizedAccount is null)
            throw new InvalidOperationException($"Can't save nothing to cache. {nameof(AuthorizedAccount)} was null");

        var accountInfo = new AuthorizationInfo(AuthorizedAccount, UserRemembered);
        await _cacher.SaveToJsonCacheAsync(_cacheFileName, accountInfo);
    }

    private void ClearAuthorizationCache() => _cacher.Clear(_cacheFileName);
}