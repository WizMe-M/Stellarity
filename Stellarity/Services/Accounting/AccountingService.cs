using System;
using System.Threading.Tasks;
using Stellarity.Database.Entities;
using Stellarity.Services.Cache;

namespace Stellarity.Services.Accounting;

public class AccountingService : CachingBase<AuthorizationInfo>
{
    private readonly string _cacheFileName;

    public AccountingService(CachingService cachingService, string cacheFileName = "auth.info")
        : base(cachingService, "Accounting/", CachingType.Json)
    {
        _cacheFileName = cacheFileName;
    }

    public bool UserRemembered { get; private set; }

    // TODO: public setter for dev only
    public Account? AuthorizedAccount { get; set; }

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
            ClearCache();
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

    private async Task<AuthorizationInfo?> LoadAuthorizationInfoAsync() => await LoadAsync(_cacheFileName);

    private async void SaveAuthorizationInfoAsync()
    {
        if (AuthorizedAccount is null)
            throw new InvalidOperationException($"Can't save nothing to cache. {nameof(AuthorizedAccount)} was null");

        var accountInfo = new AuthorizationInfo(AuthorizedAccount, UserRemembered);
        await SaveAsync(_cacheFileName, accountInfo);
    }
}