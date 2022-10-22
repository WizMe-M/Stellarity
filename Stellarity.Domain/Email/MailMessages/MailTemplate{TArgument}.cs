using MimeKit;
using MimeKit.Text;

namespace Stellarity.Domain.Email.MailMessages;

public abstract class MailTemplate<TArgument> : MailTemplate
{
    protected MailTemplate(string subject, string header) : base(subject, header)
    {
    }

    public override MimeMessage CreateMime<TBasicArgument>(Email from, Email to, TBasicArgument basicArgument)
    {
        if (basicArgument is not TArgument argument) return new MimeMessage();
        
        var mail = InstantiateFromEmails(from, to);
        SetBody(mail, argument);
        return mail;
    }

    protected void SetBody(MimeMessage mail, TArgument argument)
    {
        var mainText = CreateMainText(argument);
        mail.Body = new TextPart(TextFormat.Html) { Text = ComposeMailParts(mainText) };
    }

    protected abstract string CreateMainText(TArgument argument);
}