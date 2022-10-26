using Ninject;
using Stellarity.Database.Entities;
using Stellarity.Domain.Models;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Services;

namespace Stellarity.Domain.Abstractions;

public class SingleImageHolderModel<TEntity> : DomainModel<TEntity>
    where TEntity : SingleImageHolderEntity
{
    private Image? _singleImage;

    public SingleImageHolderModel(TEntity entity) : base(entity)
    {
        if (ImageLoaded) _singleImage = new Image(entity.SingleImageEntity!);
    }

    public bool HasImage => Entity.SingleImageId is { };
    public bool ImageLoaded => _singleImage is { } || Entity.SingleImageEntity is { };


    public async Task<byte[]?> GetImageBytesAsync()
    {
        if (!HasImage) return null;
        if (ImageLoaded) return _singleImage?.ImageBinaryData ?? Entity.SingleImageEntity?.Data;

        await LoadImageAsync();
        return _singleImage?.ImageBinaryData;
    }

    public byte[]? TryGetImageBytes()
    {
        if (!HasImage) return null;
        if (ImageLoaded) return _singleImage!.ImageBinaryData;
        return null;
    }

    public async Task LoadImageAsync()
    {
        if (!HasImage || ImageLoaded) return;
        var imageCacheService = DiContainingService.Kernel.Get<ImageCacheService>();
        var bytes = await imageCacheService.LoadImageAsync(Entity.SingleImageId);
        if (bytes is { })
        {
            Entity.SetImageFromCache(bytes);
            _singleImage = new Image(Entity.SingleImageEntity!);
            return;
        }

        Entity.LoadImage();
        _singleImage = new Image(Entity.SingleImageEntity!);
    }

    public async Task SetImageAsync(byte[] imageData, string name)
    {
        var imageCacheService = DiContainingService.Kernel.Get<ImageCacheService>();
        if (HasImage) imageCacheService.DeleteCacheFile(Entity.SingleImageId!.Value);
        Entity.SetImage(imageData, name);
        _singleImage = new Image(Entity.SingleImageEntity!);
        await imageCacheService.SaveImageAsync(_singleImage.Entity);
    }
}