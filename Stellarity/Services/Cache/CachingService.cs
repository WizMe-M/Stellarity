using Newtonsoft.Json;
using Stellarity.Extensions;

namespace Stellarity.Services.Cache;

public class CachingService
{
    private readonly string _rootFolder;

    public CachingService(string rootFolder = "Cache/")
    {
        _rootFolder = rootFolder;
    }

    /// <summary>
    /// Caches <paramref name="data"/> into the file with <paramref name="fileName"/> on the disk folder 
    /// </summary>
    /// <param name="subfolder">Directory holding a file</param>
    /// <param name="fileName">Final name of the file</param>
    /// <param name="data">Data to cache</param>
    /// <returns>Represents task of caching operation</returns>
    public Task SaveToBinaryCache<T>(string subfolder, string fileName, T data)
    {
        var subPath = Path.Combine(_rootFolder, subfolder);
        Directory.CreateDirectory(subPath);
        var path = Path.Combine(subPath, fileName);
        var bytes = data.ToBytes();
        return File.WriteAllBytesAsync(path, bytes);
    }

    /// <summary>
    /// Gets an object from specified cached file with <paramref name="fileName"/>
    /// </summary>
    /// <param name="subfolder">Directory holding a file</param>
    /// <param name="fileName">Name of the cache file</param>
    /// <returns>Represents task of reading an object from cache operation</returns>
    public Task<T?> LoadFromBinaryCache<T>(string subfolder, string fileName)
        where T : class
    {
        var path = Path.Combine(_rootFolder, subfolder, fileName);
        if (!File.Exists(path)) return Task.FromResult<T?>(null);
        var bytes = File.ReadAllBytes(path);
        var data = bytes.FromBytes<T>();
        return Task.FromResult(data)!;
    }

    /// <summary>
    /// Caches <paramref name="data"/> into the file with <paramref name="fileName"/> on the disk folder 
    /// </summary>
    /// <param name="fileName">Final name of the file</param>
    /// <param name="subfolder">Subfolder name in Cache folder</param>
    /// <param name="data">Data to cache</param>
    /// <returns>Represents task of caching operation</returns>
    public Task SaveToJsonCacheAsync<T>(string subfolder, string fileName, T data)
    {
        var subPath = Path.Combine(_rootFolder, subfolder);
        Directory.CreateDirectory(subPath);
        var path = Path.Combine(_rootFolder, fileName);
        var json = JsonConvert.SerializeObject(data);
        return File.WriteAllTextAsync(path, json);
    }

    /// <summary>
    /// Gets an object from specified cached file with <paramref name="fileName"/>
    /// </summary>
    /// <param name="subfolder">Subfolder name in Cache folder</param>
    /// <param name="fileName">Name of the cache file</param>
    /// <returns>Represents task of reading an object from cache operation</returns>
    public Task<T?> LoadFromJsonCacheAsync<T>(string subfolder, string fileName)
        where T : class
    {
        var path = Path.Combine(_rootFolder, subfolder, fileName);
        if (!File.Exists(path)) return Task.FromResult<T?>(null);
        var json = File.ReadAllText(path);
        var data = JsonConvert.DeserializeObject<T>(json);
        return Task.FromResult(data);
    }

    /// <summary>
    /// Clears all cache from root cache-folder
    /// </summary>
    public void Clear()
    {
        var root = new DirectoryInfo(_rootFolder);
        foreach (var file in root.EnumerateFiles()) Clear(file);
        foreach (var directory in root.EnumerateDirectories()) Clear(directory);
    }

    /// <summary>
    /// Clears cache from files only in specified directory
    /// </summary>
    /// <param name="directory">Target directory info</param>
    public void Clear(DirectoryInfo directory)
    {
        foreach (var file in directory.EnumerateFiles()) Clear(file);
    }

    /// <summary>
    /// Clears cache from files only in specified directory
    /// </summary>
    /// <param name="folder">Target folder name in cache root</param>
    public void ClearFolder(string folder)
    {
        var path = Path.Combine(_rootFolder, folder);
        var dir = new DirectoryInfo(path);
        foreach (var file in dir.EnumerateFiles()) Clear(file);
    }

    /// <summary>
    /// Clears specified cache-file
    /// </summary>
    /// <param name="file">Target file info</param>
    public void Clear(FileInfo file)
    {
        if (!File.Exists(file.FullName)) return;
        File.Delete(file.FullName);
    }
}