using EmailValidation;
using MailKit.Net.Smtp;
using MimeKit;
using Stellarity.Domain.Email.MailMessages;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email;

public class MailingService
{
    public const string SenderEmail = "noreply.stellarity@mail.ru";
    public const string SenderPassword = "UrWsnkJFHZ9FcubvYJZv";

    public const string SmtpHost = "smtp.mail.ru";
    public const int SmtpPort = 465;
    public const bool SmtpUseSsl = true;

    private MailTemplate _mailTemplate = null!;
    private Func<string, MimeMessage> _createMimeMessage;

    public bool Validate(string email) => EmailValidator.Validate(email);

    public async Task<EmailDeliverResult> SendEmailAsync(string email, string subject, string message)
    {
        if(Validate(email) == false) return EmailDeliverResult.NotValidEmail();
        
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Stellarity", SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(SmtpHost, SmtpPort, SmtpUseSsl);
            await client.AuthenticateAsync(SenderEmail, SenderPassword);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }

        return EmailDeliverResult.Success();
    }

    public void SetEmailTemplate(EmailTypes type)
    {
        _mailTemplate = type switch
        {
            EmailTypes.ActivateUser => new AccountActivationMail(),
            EmailTypes.ConfirmPasswordChange => new ChangePasswordConfirmationMail(),
            _ => throw new NotSupportedException()
        };
        _createMimeMessage = _mailTemplate.CreateMime;
    }
    
    

    public async Task<EmailDeliverResult> SendConfirmAccount(string email, string code)
    {
        // var isEmailValid = MailAddress.TryCreate(email, out var to);
        // if (!isEmailValid) return EmailDeliverResult.NotValidEmail();
        //
        // var mail = GetMail(to!, code);
        //
        // try
        // {
        //     await SendMailAsync(mail);
        // }
        // catch (Exception e)
        // {
        //     Debug.WriteLine(e);
        //     return EmailDeliverResult.NotDelivered();
        // }

        return EmailDeliverResult.Success();
    }

    public async Task<EmailDeliverResult> SendChangePassword(string email, string code)
    {
        // var isEmailValid = MailAddress.TryCreate(email, out var to);
        // if (!isEmailValid) return EmailDeliverResult.NotValidEmail();
        //
        // var mail = GetChangePasswordMail(to!, code);
        //
        // try
        // {
        //     await SendMailAsync(mail);
        // }
        // catch (Exception e)
        // {
        //     Debug.WriteLine(e);
        //     return EmailDeliverResult.NotDelivered();
        // }

        return EmailDeliverResult.Success();
    }

    public async Task<EmailDeliverResult> SendGameCheque(string email, PurchaseCheque cheque)
    {
        // var isEmailValid = MailAddress.TryCreate(email, out var to);
        // if (!isEmailValid) return EmailDeliverResult.NotValidEmail();
        //
        // var mail = GetGameChequeMail(to!, cheque);
        //
        // try
        // {
        //     await SendMailAsync(mail);
        // }
        // catch (Exception e)
        // {
        //     Debug.WriteLine(e);
        //     return EmailDeliverResult.NotDelivered();
        // }

        return EmailDeliverResult.Success();
    }
}