using Stellarity.Database.Entities;
using Stellarity.Domain.Email;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Tests;

public class MailTests
{
    private const string Receiver = "timkin.moxim@mail.ru";

    [Test]
    public async Task EmailValid()
    {
        var service = new MailingService();
        var result = service.Validate(Receiver);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task SendMail()
    {
        var service = new MailingService();
        
        var result = await service.SendEmailAsync(Receiver, "Test", "<p>paragraph<br/> new line</p> <center>M</center>");
        
        Assert.That(result.IsSuccessful);
    }

    [Test]
    public async Task SendActivationMail()
    {
        var service = new MailingService();
        var code = new Random().Next(100000, 999999);
        var result = await service.SendConfirmAccount(Receiver, code.ToString());
    }

    [Test]
    public async Task SendChangePasswordMail()
    {
        var service = new MailingService();
        var code = new Random().Next(100000, 999999);
        var result = await service.SendChangePassword(Receiver, code.ToString());
    }

    [Test]
    public async Task SendGameChequeMail()
    {
        var key = new Key(KeyEntity.GetUserPurchasedKeys(1)[0]);
        var mailingService = new MailingService();
        var chequeSender = new GameChequeSenderService(mailingService);
        await chequeSender.SendAsync(Receiver, key);
    }
}