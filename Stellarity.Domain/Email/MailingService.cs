using System.Net;
using System.Net.Mail;
using Stellarity.Domain.Email.MailMessages;

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

    public MailMessage GetActivationMail(in string receiverEmail, in int code)
    {
        var to = new MailAddress(receiverEmail);
        var mailTemplate = new AccountActivationMail(_from, to, code);
        return mailTemplate.GetMailMessage();
    }

    public Task SendMailAsync(MailMessage mail) => _smtp.SendMailAsync(mail);
}