using Avalonia.Media.Imaging;
using Stellarity.Avalonia.Models;
using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Avalonia.Extensions;

public static class ImageHolderModelExtensions
{
    public static async Task<Bitmap?> GetImageBitmapAsync<TEntity>(this SingleImageHolderModel<TEntity> holder)
        where TEntity : SingleImageHolderEntity
    {
        var bytes = await holder.GetImageBytesAsync();
        return bytes.ToBitmap();
    }
    
    public static Bitmap? TryGetImageBitmap<TEntity>(this SingleImageHolderModel<TEntity> holder)
        where TEntity : SingleImageHolderEntity
    {
        var bytes = holder.TryGetImageBytes();
        return bytes.ToBitmap();
    }

    public static Bitmap GetImageBitmapOrDefault<TEntity>(this SingleImageHolderModel<TEntity> holder) 
        where TEntity : SingleImageHolderEntity
    {
        var bitmap = holder.TryGetImageBitmap();
        return bitmap ?? ImagePlaceholder.GetBitmap();
    }
}