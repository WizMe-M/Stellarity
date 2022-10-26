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
        var newImage = new ImageEntity
        {
            Name = name, 
            Data = imageData
        };
        context.Images.Add(newImage);
        var oldImage = context.Images.FirstOrDefault(img => img.Guid == SingleImageId);
        if (oldImage is { }) context.Images.Remove(oldImage);
        context.SaveChanges();

        context.Attach(this);
        SingleImageId = newImage.Guid;
        context.Update(this);

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