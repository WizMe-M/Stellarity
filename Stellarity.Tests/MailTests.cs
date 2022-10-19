using Stellarity.Domain.Email;

namespace Stellarity.Tests;

public class MailTests
{
    private const string Receiver = "timkin.moxim@mail.ru";

    [Test]
    public async Task SendActivationMail()
    {
        var service = new MailingService();

        var result = await service.SendConfirmAccount(Receiver);
    }

    [Test]
    public async Task SendChangePasswordMail()
    {
        var service = new MailingService();

        var result = await service.SendChangePassword(Receiver);
    }
}