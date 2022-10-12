namespace Stellarity.Database.Entities;

public abstract class SingleImageHolderEntity : IEntity
{
    public Guid? SingleImageId { get; private set; }

    public ImageEntity? SingleImageEntity { get; private set; }

    public void LoadImage()
    {
        if (SingleImageId is null) return;

        using var context = new StellarityContext();
        var image = context.Images.FirstOrDefault(img => img.Guid == SingleImageId);
        SingleImageEntity = image;
    }

    /// <summary>
    /// Гарантированно записывает изображение в бд и
    /// обновляет изображение (загружает) у текущей сущности 
    /// </summary>
    public void SetImage(byte[] imageData, string name = "unknown")
    {
        using var context = new StellarityContext();
        var newImage = new ImageEntity(name, imageData);
        context.Images.Add(newImage);
        context.SaveChanges();

        var oldImage = context.Images.FirstOrDefault(img => img.Guid == SingleImageId);
        var entity = context.Attach(this).Entity;
        entity.SingleImageId = newImage.Guid;
        context.Update(entity);

        if (oldImage is { }) context.Images.Remove(oldImage);

        context.SaveChanges();
    }

    public void SetImageFromCache(byte[] imageData)
    {
        SingleImageEntity = new ImageEntity
        {
            Guid = SingleImageId!.Value,
            Data = imageData,
            Name = "Unknown. Loaded from cache"
        };
    }
}