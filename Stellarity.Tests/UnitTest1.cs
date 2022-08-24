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
}