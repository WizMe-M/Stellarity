using System.Text;
using MimeKit;

namespace Stellarity.Domain.Email.MailMessages;

public abstract class MailTemplate
{
    public const string StandardFooter = "<center><p>Спасибо, что пользуетесь нашим сервисом</p></center>";

    protected MailTemplate(string subject, string header, string footer = StandardFooter)
    {
        To = new Email("unknown", "undefined@mail"); 
    
        Subject = subject;
        Header = header;
        Footer = footer;
    }

    protected Email To { get; private set; }

    public string Subject { get; }
    public string Header { get; }
    public string Footer { get; }

    public abstract MimeMessage CreateMime<TArgument>(Email from, Email to, TArgument basicArgument);

    protected MimeMessage InstantiateFromEmails(Email from, Email to)
    {
        To = to;
        
        var mail = new MimeMessage();
        mail.Subject = Subject;
        mail.From.Add(new MailboxAddress(from.Name, from.Address));
        mail.To.Add(new MailboxAddress(to.Name, to.Address));
        return mail;
    }

    protected string ComposeMailParts(string mainText)
    {
        var builder = new StringBuilder(4);
        builder.Append(Header);
        builder.Append(mainText);
        builder.Append(Footer);
        return builder.ToString();
    }
}