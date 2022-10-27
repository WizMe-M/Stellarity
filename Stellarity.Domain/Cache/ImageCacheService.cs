using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Cache;

public class ImageCacheService : CachingBaseService<byte[]>
{
    public ImageCacheService(Cacher cacher) : base(cacher, "Images/", CachingType.Binary)
    {
    }

    public Task SaveImageAsync(Image image)
    {
        var guidStr = image.Guid.ToString();
        var hashedFileName = MD5Hasher.Encrypt(guidStr);
        var data = image.BinaryData;
        return SaveAsync(hashedFileName, data);
    }

    public async Task<byte[]?> LoadImageAsync(Guid? imageGuid)
    {
        if (imageGuid is null) return null;

        var guidStr = imageGuid.Value.ToString();
        var hashedFileName = MD5Hasher.Encrypt(guidStr);
        byte[]? result;
        try
        {
            result = await LoadAsync(hashedFileName);
        }
        catch (NullReferenceException)
        {
            result = null;
        }

        return result;
    }

    public void DeleteCacheFile(Guid imageGuid)
    {
        var guidStr = imageGuid.ToString();
        var hashedFileName = MD5Hasher.Encrypt(guidStr);
        DeleteFile(hashedFileName);
    }
}