using Stellarity.Database.Entities;
using Stellarity.Domain.Services.Cache;
using Stellarity.Extensions;

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
        var hashedFileName = guidStr.CreateMD5();
        var data = image.Data;
        return SaveAsync(hashedFileName, data);
    }

    public async Task<byte[]?> LoadImageAsync(Guid? imageGuid)
    {
        if (imageGuid is null) return null;

        var guidStr = imageGuid.Value.ToString();
        var hashedFileName = guidStr.CreateMD5();
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