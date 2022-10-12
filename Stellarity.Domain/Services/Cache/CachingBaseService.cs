namespace Stellarity.Domain.Services.Cache;

public abstract class CachingBaseService<TCacheDataType> where TCacheDataType : class
{
    private readonly Cacher _cacher;
    private readonly string _cacheSubfolder;
    private readonly CachingType _settings;

    protected CachingBaseService(Cacher cacher, string cacheSubfolder, CachingType settings)
    {
        _cacher = cacher;
        _cacheSubfolder = cacheSubfolder;
        _settings = settings;
    }

    protected Task SaveAsync(string fileName, TCacheDataType data) => _settings switch
    {
        CachingType.Binary => _cacher.SaveToBinaryCache(_cacheSubfolder, fileName, data),
        CachingType.Json => _cacher.SaveToJsonCacheAsync(_cacheSubfolder, fileName, data),
        _ => throw new InvalidOperationException("Only binary and json caching allowed")
    };

    protected async Task<TCacheDataType?> LoadAsync(string file) => _settings switch
    {
        CachingType.Binary => await _cacher.LoadFromBinaryCache<TCacheDataType>(_cacheSubfolder, file),
        CachingType.Json => await _cacher.LoadFromJsonCacheAsync<TCacheDataType>(_cacheSubfolder, file),
        _ => throw new InvalidOperationException("Only binary and json caching allowed")
    };

    protected void ClearCache() => _cacher.ClearFolder(_cacheSubfolder);

    protected void DeleteFile(string fileName)
    {
        var path = Path.Join(_cacher.RootFolder, _cacheSubfolder, fileName);
        if (File.Exists(path)) File.Delete(path);
    }
}