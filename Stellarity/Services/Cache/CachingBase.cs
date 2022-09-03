using System;
using System.Threading.Tasks;

namespace Stellarity.Services.Cache;

public abstract class CachingBase<TCacheDataType> where TCacheDataType : class
{
    private readonly CachingService _cachingService;
    private readonly string _cacheSubfolder;
    private readonly CachingType _settings;

    protected CachingBase(CachingService cachingService, string cacheSubfolder, CachingType settings)
    {
        _cachingService = cachingService;
        _cacheSubfolder = cacheSubfolder;
        _settings = settings;
    }

    protected Task SaveAsync(string fileName, TCacheDataType data) => _settings switch
    {
        CachingType.Binary => _cachingService.SaveToBinaryCache(_cacheSubfolder, fileName, data),
        CachingType.Json => _cachingService.SaveToJsonCacheAsync(_cacheSubfolder, fileName, data),
        _ => throw new InvalidOperationException("Only binary and json caching allowed")
    };

    protected async Task<TCacheDataType?> LoadAsync(string file) => _settings switch
    {
        CachingType.Binary => await _cachingService.LoadFromBinaryCache<TCacheDataType>(_cacheSubfolder, file),
        CachingType.Json => await _cachingService.LoadFromJsonCacheAsync<TCacheDataType>(_cacheSubfolder, file),
        _ => throw new InvalidOperationException("Only binary and json caching allowed")
    };

    protected void ClearCache() => _cachingService.ClearFolder(_cacheSubfolder);
}