using System.Net.Mail;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email.MailMessages;

public class GameChequeMail : MailTemplate
{
    private const string Subject = "Чек";
    private const string Header = "<center><h1>Чек на покупку игры</h1></center>";

    private const string BodyTemplate = @"
<p>Здравствуйте, <b>{0}</b>. Вы получили это письмо, потому что совершили покупку в Stellarity.</p>
<p>Дата приобретения: <b>{1}</b><br/>
Название: <b>{2}</b><br/>
Цена: {3} руб.
НДС: {4}% ({5} руб.)</p>
<center>Ваш ключ: <b>{6}</b></center>";

    private readonly PurchaseCheque _cheque;

    public GameChequeMail(MailAddress from, MailAddress to, PurchaseCheque cheque)
        : base(Subject, Header, BodyTemplate, from, to)
    {
        _cheque = cheque;
    }

    public virtual MailMessage GetMailMessage()
    {
        var mail = InitMailMessage();
        var mainText = CreateMainTextFromTemplate(_cheque.BuyerEmail, _cheque.PurchaseDate,
            _cheque.Title, _cheque.Total, PurchaseCheque.VatPercent, _cheque.VatFromTotal, _cheque.KeyValue);
        mail.Body = AppendMailPartsToMainText(mainText);
        return mail;
    }
}