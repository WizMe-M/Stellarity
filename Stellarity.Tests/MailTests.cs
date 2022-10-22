using Stellarity.Database.Entities;
using Stellarity.Domain.Email;
using Stellarity.Domain.Email.MailMessages;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Tests;

public class MailTests
{
    private const string Receiver = "timkin.moxim@mail.ru";

    [Test]
    public void MailTemplates()
    {
        TestDelegate CreateCommonBase()
        {
            return () =>
            {
                const EmailType type = EmailType.AccountActivation;

                MailTemplate commonBase = type switch
                {
                    EmailType.AccountActivation => new AccountActivationMail(),
                    EmailType.PasswordChange => new ChangePasswordConfirmationMail(),
                    EmailType.PurchaseCheque => new GameChequeMail(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            };
        }

        Assert.DoesNotThrow(CreateCommonBase());
    }

    [Test]
    public void EmailValid()
    {
        var service = new MailingService();
        var result = MailingService.Validate(Receiver);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task SendMail()
    {
        var service = new MailingService();
        var result = await service.SendEmailAsync(Receiver, EmailType.AccountActivation, 42);
        Assert.That(result.IsSuccessful);
    }

    [Test]
    public async Task SendActivationMail()
    {
        var service = new MailingService();
        var code = new Random().Next(100000, 999999);
        var result = await service.SendAccountActivationCodeAsync(Receiver, code);
        Assert.That(result.IsSuccessful);
    }

    [Test]
    public async Task SendChangePasswordMail()
    {
        var service = new MailingService();
        var code = new Random().Next(100000, 999999);
        var result = await service.SendChangePasswordConfirmationCodeAsync(Receiver, code);
        Assert.That(result.IsSuccessful);
    }

    [Test]
    public async Task SendGameChequeMail()
    {
        var key = new Key(KeyEntity.GetUserPurchasedKeys(1)[0]);
        var mailingService = new MailingService();
        var chequeSender = new GameChequeSenderService(mailingService);
        var result = await chequeSender.SendAsync(Receiver, key);
        Assert.That(result.IsSuccessful);
    }
}