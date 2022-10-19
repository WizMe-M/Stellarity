using Stellarity.Domain.Email;

namespace Stellarity.Tests;

public class MailTests
{
    private const string Receiver = "timkin.moxim@mail.ru";

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
        const string key = "AAAAA-BBBBB-CCCCC";
        var service = new MailingService();
        var result = await service.SendGameCheque(Receiver, key);
    }
}