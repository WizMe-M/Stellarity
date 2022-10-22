using System.Net.Mail;
using System.Text;
using MimeKit;

namespace Stellarity.Domain.Email.MailMessages;

public abstract class MailTemplate
{
    private const string Footer = "<center><p>Спасибо, что пользуетесь нашим сервисом</p></center>";

    private readonly string _subject;
    private readonly string _header;
    private readonly string _mailTemplate;

    private readonly MailAddress _from;
    private readonly MailAddress _to;

    protected MailTemplate()
    {
    }

    protected MailTemplate(string subject, string header, string mailTemplate, MailAddress from, MailAddress to)
    {
        _subject = subject;
        _header = header;
        _mailTemplate = mailTemplate;
        _from = from;
        _to = to;
    }

    protected MailMessage InitMailMessage()
    {
        var mail = new MailMessage(_from, _to)
        {
            IsBodyHtml = true,
            Subject = _subject
        };

        return mail;
    }

    protected string AppendMailPartsToMainText(string body)
    {
        var builder = new StringBuilder(4);
        builder.Append(_header);
        builder.Append(body);
        builder.Append(Footer);
        return builder.ToString();
    }

    protected string CreateMainTextFromTemplate(params object[] args) => string.Format(_mailTemplate, args);

    public MimeMessage CreateMime(string email)
    {
        return new MimeMessage { To = { new MailboxAddress("", email) }};
    }
}