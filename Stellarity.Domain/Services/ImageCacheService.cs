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

    public Task SaveAvatarAsync(ImageEntity avatar)
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