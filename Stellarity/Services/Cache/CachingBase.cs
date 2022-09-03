using System.Threading.Tasks;

namespace Stellarity.Services.Cache;

public abstract class CachingBase<TCacheDataType>
    where TCacheDataType : class
{
    protected readonly CachingService CachingService;
    protected readonly string CacheSubfolder;

    protected CachingBase(CachingService cachingService, string cacheSubfolder)
    {
        CachingService = cachingService;
        CacheSubfolder = cacheSubfolder;
    }

    protected abstract Task SaveAsync(TCacheDataType data);

    protected abstract Task<TCacheDataType> LoadAsync(string file);

    public void ClearCache() => CachingService.ClearFolder(CacheSubfolder);
}