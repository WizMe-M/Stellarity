using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Image : DomainModel<ImageEntity>
{
    public Image(ImageEntity entity) : base(entity)
    {
    }

    public Guid Guid => Entity.Guid;
    public byte[] BinaryData => Entity.Data;
    public string Name => Entity.Name;
}