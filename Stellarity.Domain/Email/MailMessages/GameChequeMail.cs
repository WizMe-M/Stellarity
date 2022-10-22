using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email.MailMessages;

public class GameChequeMail : MailTemplate<PurchaseCheque>
{
    private const string ConstSubject = "Чек";
    private const string ConstHeader = "<center><h1>Чек на покупку игры</h1></center>";

    public GameChequeMail() : base(ConstSubject, ConstHeader)
    {
    }

    protected override string CreateMainText(PurchaseCheque argument)
    {
        return @$"<p>Здравствуйте, <b>{argument.BuyerEmail}</b>. 
        Вы получили это письмо, потому что совершили покупку в Stellarity.</p>
        <p>Дата приобретения: <b>{argument.PurchaseDate}</b><br/>
        Название: <b>{argument.Title}</b><br/>
        Цена: {argument.Total} руб.
        НДС: {PurchaseCheque.VatPercent}% ({argument.VatFromTotal} руб.)</p>
        <center>Ваш ключ: <b>{argument.KeyValue}</b></center>";
    }
}