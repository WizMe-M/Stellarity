using System.Net.Mail;

namespace Stellarity.Domain.Email.MailMessages;

public class GameChequeMail : MailTemplate
{
    private const string Subject = "Чек";
    private const string Header = "<center><h1>Чек на покупку игры</h1></center>";

    private const string BodyTemplate = @"
<p>Здравствуйте, <b>{0}</b>. Для подтверждения вашего аккаунта введите полученный код в приложении. 
Если вы не делали этого, просто игнорируйте данное письмо.</p>
<center>Код подтверждения: <b>{1}</b></center>";

    private readonly string _to;

    /// <summary>
    /// Валидный ключ от игры
    /// </summary>
    private readonly string _key;

    public GameChequeMail(MailAddress from, MailAddress to, string key)
        : base(Subject, Header, BodyTemplate, from, to)
    {
        _key = key;
        _to = to.Address;
    }

    public override MailMessage GetMailMessage()
    {
        var mail = InitMailMessage();
        var mainText = CreateMainTextFromTemplate(_to, _key);
        mail.Body = AppendMailPartsToMainText(mainText);
        return mail;
    }
}