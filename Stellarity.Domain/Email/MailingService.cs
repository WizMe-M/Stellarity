using EmailValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Stellarity.Domain.Email.MailMessages;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email;

public class MailingService
{
    private readonly string _senderEmail;
    private readonly string _senderPassword;

    private const string SmtpHost = "smtp.mail.ru";
    private const int SmtpPort = 465;

    private readonly Email _sender;

    public MailingService()
    {
        (_senderEmail, _senderPassword) = LoadSender();
        _sender = new Email("Stellarity", _senderEmail);
    }

    #region external methods

    public Task<EmailDeliverResult> SendAccountActivationCodeAsync(string email, int code) =>
        SendEmailAsync(email, EmailType.AccountActivation, code);

    public Task<EmailDeliverResult> SendChangePasswordConfirmationCodeAsync(string email, int code) =>
        SendEmailAsync(email, EmailType.PasswordChange, code);

    public Task<EmailDeliverResult> SendPurchaseChequeAsync(string email, PurchaseCheque cheque) =>
        SendEmailAsync(email, EmailType.PurchaseCheque, cheque);

    #endregion

    #region internal methods

    private static (string email, string password) LoadSender()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Join(currentDirectory, "sender.txt");
        var data = File.ReadAllText(path);
        var sender = data.Split(' ');
        return (sender[0], sender[1]);
    }

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

    private async Task SendMimeAsync(MimeMessage mime)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_senderEmail, _senderPassword);
        await client.SendAsync(mime);

        await client.DisconnectAsync(true);
    }

    #endregion
}