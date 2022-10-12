using Stellarity.Database.Entities;

namespace Stellarity.Tests;

public class DatabaseTests
{
    [Test]
    public void EntitySetImage()
    {
        var entity = AccountEntity.GetAdmin();
        entity.SetImage(new byte[] { 2, 4, 0, 8 });
    }

    [Test]
    public void EntityLoadImage()
    {
        var entity = AccountEntity.GetAdmin();
        entity.LoadImage();
    }
}