using Stellarity.Database.Entities;
using Stellarity.Domain.Cryptography;
using Stellarity.Domain.Services.Cache;

namespace Stellarity.Domain.Services;

public class ImageCacheService : CachingBaseService<byte[]>
{
    public ImageCacheService(Cacher cacher)
        : base(cacher, "Images/", CachingType.Binary)
    {
    }

    public Task SaveImageAsync(ImageEntity image)
    {
        var guidStr = image.Guid.ToString();
        var hashedFileName = MD5Hasher.Encrypt(guidStr);
        var data = image.Data;
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
}