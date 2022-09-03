using System;
using System.Threading.Tasks;

namespace Stellarity.Services.Cache;

public abstract class CachingBase<TCacheDataType>
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

    protected Task SaveAsync(TCacheDataType data, string fileName)
    {
        return _settings switch
        {
            CachingType.Binary => _cachingService.SaveToBinaryCache(fileName, data),
            CachingType.Json => _cachingService.SaveToJsonCacheAsync(fileName, _cacheSubfolder, data),
            _ => throw new InvalidOperationException("Only binary and json caching allowed")
        };
    }

    protected Task<TCacheDataType?> LoadAsync(string file)
    {
        return _settings switch
        {
            CachingType.Binary => _cachingService.LoadFromJsonCacheAsync<TCacheDataType>(file, _cacheSubfolder),
            CachingType.Json => _cachingService.LoadFromJsonCacheAsync<TCacheDataType>(file, _cacheSubfolder),
            _ => throw new InvalidOperationException("Only binary and json caching allowed")
        };
    }

    protected void ClearCache() => _cachingService.ClearFolder(_cacheSubfolder);
}