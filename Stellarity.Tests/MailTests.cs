using Stellarity.Domain.Email;

namespace Stellarity.Tests;

public class MailTests
{
    [Test]
    public async Task CreateActivationMail()
    {
        const string receiver = "timkin.moxim@mail.ru";
        var code = new Random().Next(100000, 999999);
        var service = new MailingService();

        var mail = service.GetActivationMail(receiver, code);
        await service.SendMailAsync(mail);
    }
}