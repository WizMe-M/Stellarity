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

    private readonly Email _sender = new("Stellarity", SenderEmail);

    #region external methods

    public Task<EmailDeliverResult> SendAccountActivationCodeAsync(string email, int code) =>
        SendEmailAsync(email, EmailType.AccountActivation, code);

    public Task<EmailDeliverResult> SendChangePasswordConfirmationCodeAsync(string email, int code) =>
        SendEmailAsync(email, EmailType.PasswordChange, code);

    public Task<EmailDeliverResult> SendPurchaseChequeAsync(string email, PurchaseCheque cheque) =>
        SendEmailAsync(email, EmailType.PurchaseCheque, cheque);

    #endregion

    #region internal methods

    public async Task<EmailDeliverResult> SendEmailAsync<TArgument>(string email, EmailType type, TArgument argument)
    {
        if (Validate(email) == false) return EmailDeliverResult.NotValidEmail();
        var to = new Email(email, email);

        var mailTemplate = SelectTemplate(type);
        var mail = mailTemplate.CreateMime(_sender, to, argument);
        await SendMimeAsync(mail);

        return EmailDeliverResult.Success();
    }

    public static bool Validate(string email) => EmailValidator.Validate(email);

    private static MailTemplate SelectTemplate(EmailType type)
    {
        return type switch
        {
            EmailType.AccountActivation => new AccountActivationMail(),
            EmailType.PasswordChange => new ChangePasswordConfirmationMail(),
            EmailType.PurchaseCheque => new GameChequeMail(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static async Task SendMimeAsync(MimeMessage mime)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(SmtpHost, SmtpPort, SmtpUseSsl);
        await client.AuthenticateAsync(SenderEmail, SenderPassword);
        await client.SendAsync(mime);

        await client.DisconnectAsync(true);
    }

    #endregion
}