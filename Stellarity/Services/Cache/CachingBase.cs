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

    protected Task SaveAsync(string fileName, TCacheDataType data)
    {
        return _settings switch
        {
            CachingType.Binary => _cachingService.SaveToBinaryCache(_cacheSubfolder, fileName, data),
            CachingType.Json => _cachingService.SaveToJsonCacheAsync(_cacheSubfolder, fileName, data),
            _ => throw new InvalidOperationException("Only binary and json caching allowed")
        };
    }

    protected Task<TCacheDataType?> LoadAsync(string file)
    {
        return _settings switch
        {
            CachingType.Binary => _cachingService.LoadFromBinaryCache<TCacheDataType>(_cacheSubfolder, file),
            CachingType.Json => _cachingService.LoadFromJsonCacheAsync<TCacheDataType>(_cacheSubfolder, file),
            _ => throw new InvalidOperationException("Only binary and json caching allowed")
        };
    }

    protected void ClearCache() => _cachingService.ClearFolder(_cacheSubfolder);
}