using Stellarity.Database.Entities;
using Stellarity.Extensions;
using Stellarity.Services.Cache;

namespace Stellarity.Services;

public class ImageCacheService : CachingBase<byte[]>
{
    public ImageCacheService(CachingService cachingService) 
        : base(cachingService, "Images/", CachingType.Binary)
    {
    }

    public Task SaveAvatarAsync(Image avatar)
    {
        var guidStr = avatar.Guid.ToString();
        var hashedFileName = guidStr.CreateMD5();
        var data = avatar.Data;
        return SaveAsync(hashedFileName, data);
    }

    public async Task<byte[]?> LoadAvatarAsync(Guid? avatarGuid)
    {
        if (avatarGuid is null) return null;

        var guidStr = avatarGuid.Value.ToString();
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