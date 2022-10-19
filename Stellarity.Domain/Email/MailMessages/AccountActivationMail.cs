using System.Net.Mail;

namespace Stellarity.Domain.Email.MailMessages;

public class AccountActivationMail : MailTemplate
{
    private const string Subject = "Активация аккаунта";
    private const string Header = "<center><h1>Активация аккаунта</h1></center>";

    private const string BodyTemplate = @"
<p>Здравствуйте, <b>{0}</b>. Для подтверждения вашего аккаунта введите полученный код в приложении. 
Если вы не делали этого, просто игнорируйте данное письмо.</p>
<center>Код подтверждения: <b>{1}</b></center>";

    private readonly string _activationCode;
    private readonly string _to;

    public AccountActivationMail(MailAddress from, MailAddress to, string activationCode)
        : base(Subject, Header, BodyTemplate, from, to)
    {
        _activationCode = activationCode;
        _to = to.Address;
    }

    public override MailMessage GetMailMessage()
    {
        var mail = InitMailMessage();
        var mainText = CreateMainTextFromTemplate(_to, _activationCode);
        mail.Body = AppendMailPartsToMainText(mainText);
        return mail;
    }
}