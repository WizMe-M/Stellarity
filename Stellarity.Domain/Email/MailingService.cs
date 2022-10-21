using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Stellarity.Domain.Email.MailMessages;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email;

public class MailingService
{
    private readonly SmtpClient _smtp;
    private readonly MailAddress _from;
    public const string SenderEmail = "noreply.stellarity@mail.ru";
    public const string SenderPassword = "UrWsnkJFHZ9FcubvYJZv";

    public MailingService()
    {
        _smtp = new SmtpClient("smtp.mail.ru")
        {
            Credentials = new NetworkCredential(SenderEmail, SenderPassword),
            EnableSsl = true
        };
        _from = new MailAddress(SenderEmail, "Stellarity");
    }

    public async Task<EmailDeliverResult> SendConfirmAccount(string email, string code)
    {
        var isEmailValid = MailAddress.TryCreate(email, out var to);
        if (!isEmailValid) return EmailDeliverResult.NotValidEmail();

        var mail = GetActivationMail(to!, code);

        try
        {
            await SendMailAsync(mail);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return EmailDeliverResult.NotDelivered();
        }

        return EmailDeliverResult.Success();
    }

    public async Task<EmailDeliverResult> SendChangePassword(string email, string code)
    {
        var isEmailValid = MailAddress.TryCreate(email, out var to);
        if (!isEmailValid) return EmailDeliverResult.NotValidEmail();

        var mail = GetChangePasswordMail(to!, code);

        try
        {
            await SendMailAsync(mail);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return EmailDeliverResult.NotDelivered();
        }

        return EmailDeliverResult.Success();
    }

    public async Task<EmailDeliverResult> SendGameCheque(string email, PurchaseCheque cheque)
    {
        var isEmailValid = MailAddress.TryCreate(email, out var to);
        if (!isEmailValid) return EmailDeliverResult.NotValidEmail();

        var mail = GetGameChequeMail(to!, cheque);

        try
        {
            await SendMailAsync(mail);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return EmailDeliverResult.NotDelivered();
        }

        return EmailDeliverResult.Success();
    }

    public MailMessage GetActivationMail(in MailAddress to, in string code)
    {
        var mailTemplate = new AccountActivationMail(_from, to, code);
        return mailTemplate.GetMailMessage();
    }

    public MailMessage GetChangePasswordMail(in MailAddress to, in string code)
    {
        var mailTemplate = new ChangePasswordConfirmationMail(_from, to, code);
        return mailTemplate.GetMailMessage();
    }

    public MailMessage GetGameChequeMail(in MailAddress to, in PurchaseCheque cheque)
    {
        var mailTemplate = new GameChequeMail(_from, to, cheque);
        return mailTemplate.GetMailMessage();
    }

    public Task SendMailAsync(MailMessage mail) => _smtp.SendMailAsync(mail);
}