using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stellarity.Extensions;

namespace Stellarity.Services;

public class CachingService
{
    private readonly string _cacheRootFolder;

    public CachingService(string cacheRootFolder = "Cache/")
    {
        _cacheRootFolder = cacheRootFolder;
    }

    /// <summary>
    /// Caches <paramref name="data"/> into the file with <paramref name="fileName"/> on the disk folder 
    /// </summary>
    /// <param name="fileName">Final name of the file</param>
    /// <param name="data">Data to cache</param>
    /// <returns>Represents task of caching operation</returns>
    public Task SaveToBinaryCache<T>(string fileName, T data)
    {
        Directory.CreateDirectory(_cacheRootFolder);
        var path = Path.Combine(_cacheRootFolder, fileName);
        var bytes = data.ToBytes();
        return File.WriteAllBytesAsync(path, bytes);
    }

    /// <summary>
    /// Gets an object from specified cached file with <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Name of the cache file</param>
    /// <returns>Represents task of reading an object from cache operation</returns>
    public Task<T?> LoadFromBinaryCache<T>(string fileName)
        where T : class
    {
        var path = Path.Combine(_cacheRootFolder, fileName);
        if (!File.Exists(path)) return Task.FromResult<T?>(null);
        var bytes = File.ReadAllBytes(path);
        var data = bytes.FromBytes<T>();
        return Task.FromResult(data)!;
    }

    /// <summary>
    /// Caches <paramref name="data"/> into the file with <paramref name="fileName"/> on the disk folder 
    /// </summary>
    /// <param name="fileName">Final name of the file</param>
    /// <param name="data">Data to cache</param>
    /// <returns>Represents task of caching operation</returns>
    public Task SaveToJsonCacheAsync<T>(string fileName, T data)
        where T : class
    {
        Directory.CreateDirectory(_cacheRootFolder);
        var path = Path.Combine(_cacheRootFolder, fileName);
        var json = JsonConvert.SerializeObject(data);
        return File.WriteAllTextAsync(path, json);
    }

    /// <summary>
    /// Gets an object from specified cached file with <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Name of the cache file</param>
    /// <returns>Represents task of reading an object from cache operation</returns>
    public Task<T?> LoadFromJsonCacheAsync<T>(string fileName)
        where T : class
    {
        var path = Path.Combine(_cacheRootFolder, fileName);
        if (!File.Exists(path)) return Task.FromResult<T?>(null);
        var json = File.ReadAllText(path);
        var data = JsonConvert.DeserializeObject<T>(json);
        return Task.FromResult(data);
    }

    /// <summary>
    /// Clears all cache from cache-folder
    /// </summary>
    public void Clear()
    {
        var directory = new DirectoryInfo(_cacheRootFolder);
        var files = directory.GetFiles();
        foreach (var file in files)
            Clear(file.Name);
    }

    /// <summary>
    /// Clears specified cache-file
    /// </summary>
    /// <param name="fileName">Name of the cache-file</param>
    public void Clear(string fileName)
    {
        var path = Path.Combine(_cacheRootFolder, fileName);
        if (!File.Exists(path)) return;
        File.Delete(path);
    }
}