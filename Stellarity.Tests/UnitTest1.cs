using Stellarity.Database;
using Stellarity.Extensions;

namespace Stellarity.Tests;

public class ByteExtensionsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleObjectCorrectConvert()
    {
        var expected = true;
        var b = true;
        var bytes = b.ToBytes();
        var actual = bytes.FromBytes<bool>();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void ChangedUserAndEntryEntityAreSame()
    {
        using var context = new StellarisContext();
        var user = context.Users.First();
        user.Nickname = "TEST NICK NAME";
        var entity = context.Entry(user).Entity;
        Assert.That(entity, Is.SameAs(user));
    }
}