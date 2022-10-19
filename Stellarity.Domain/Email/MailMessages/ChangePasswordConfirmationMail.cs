using System.Net.Mail;

namespace Stellarity.Domain.Email.MailMessages;

public class ChangePasswordConfirmationMail : MailTemplate
{
    private const string Subject = "Смена пароля";
    private const string Header = "<h1>Смена пароля</h1>";

    private const string ChangePasswordTemplate = @"
<p>Здравствуйте, <b>{0}</b>. Вы получили это сообщение, потому что мы получили от Вас запрос на смену пароля.<br/>
Если вы не делали этого, просто игнорируйте данное письмо.<br/>
Код подтверждения: <b>{1}</b></p>";

    private readonly int _confirmationCode;

    public ChangePasswordConfirmationMail(MailAddress from, MailAddress to, int confirmationCode)
        : base(Subject, Header, ChangePasswordTemplate, from, to)
    {
        _confirmationCode = confirmationCode;
    }

    public override MailMessage GetMailMessage()
    {
        var mail = InitMailMessage();
        var mainText = CreateMainTextFromTemplate(_confirmationCode);
        mail.Body = AppendMailPartsToMainText(mainText);
        return mail;
    }
}