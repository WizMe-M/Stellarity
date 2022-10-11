using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Image : DomainModel<ImageEntity>
{
    public Image(ImageEntity entity) : base(entity)
    {
        Name = entity.Name;
        ImageBinaryData = entity.Data;
    }

    public byte[] ImageBinaryData { get; }
    public string Name { get; }
}