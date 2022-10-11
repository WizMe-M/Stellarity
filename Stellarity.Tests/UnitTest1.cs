using Stellarity.Database.Entities;
using Stellarity.Domain.Models;
using Stellarity.Extensions;

namespace Stellarity.Tests;

public class Tests
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
    public void ModelFromEntity()
    {
        var entity = new AccountEntity()
        {
            Nickname = "Nick",
            Email = "test@mail.ru",
            Password = "P@ssw0rd",
            Balance = 200
        };
        var acc = new Account(entity);
    }
}