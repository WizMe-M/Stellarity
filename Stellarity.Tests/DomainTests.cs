using Stellarity.Database.Entities;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Tests;

public class DomainTests
{
    [SetUp]
    public void SetUpDi()
    {
        DiContainingService.Initialize();
    }

    [Test]
    public async Task AccountGetImageBytes()
    {
        DiContainingService.Initialize();

        var ent = AccountEntity.GetAdmin();
        var acc = new Account(ent);
        var bytes = await acc.GetImageBytes();
    }

    [Test]
    public async Task AccountSetImage()
    {
        var ent = AccountEntity.GetAdmin();
        var acc = new Account(ent);
        var bytes = new byte[] { 2, 0, 7, 7 };
        await acc.SetImageAsync(bytes, "unknown shit");
    }

    [Test]
    public async Task AccountLoadImage()
    {
        var ent = AccountEntity.GetAdmin();
        var acc = new Account(ent);
        await acc.LoadImageAsync();
    }
}