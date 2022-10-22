using System.Net.Mail;

namespace Stellarity.Domain.Email.MailMessages;

public class ChangePasswordConfirmationMail : MailTemplate
{
    private const string Subject = "Смена пароля";
    private const string Header = "<center><h1>Смена пароля</h1></center>";

    private const string ChangePasswordTemplate = @"
<p>Здравствуйте, <b>{0}</b>. Вы получили это сообщение, потому что мы получили от Вас запрос на смену пароля.<br/>
Если вы не делали этого, просто игнорируйте данное письмо.<br/>
<center>Код подтверждения: <b>{1}</b></center>";

    private readonly string _receiver;
    private readonly string _confirmationCode;

    public ChangePasswordConfirmationMail()
    {
    }

    public ChangePasswordConfirmationMail(MailAddress from, MailAddress to, string confirmationCode)
        : base(Subject, Header, ChangePasswordTemplate, from, to)
    {
        _confirmationCode = confirmationCode;
        _receiver = to.Address;
    }

    public virtual MailMessage GetMailMessage()
    {
        var mail = InitMailMessage();
        var mainText = CreateMainTextFromTemplate(_receiver, _confirmationCode);
        mail.Body = AppendMailPartsToMainText(mainText);
        return mail;
    }
}