using Avalonia.Media.Imaging;
using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Avalonia.Extensions;

public static class ImageHolderModelExtensions
{
    public static async Task<Bitmap?> GetImageBitmap<TEntity>(this SingleImageHolderModel<TEntity> image)
        where TEntity : SingleImageHolderEntity
    {
        var bytes = await image.GetImageBytes();
        return bytes.ToBitmap();
    }
}