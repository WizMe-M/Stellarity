namespace Stellarity.Domain.Email.MailMessages;

public class AccountActivationMail : MailTemplate<int>
{
    private const string ConstSubject = "Активация аккаунта";
    private const string ConstHeader = "<center><h1>Активация аккаунта</h1></center>";

    public AccountActivationMail() : base(ConstSubject, ConstHeader)
    {
    }

    protected override string CreateMainText(int argument)
    {
        return @$"<p>Здравствуйте, <b>{To.Name}</b>. 
        Для подтверждения вашего аккаунта введите полученный код в приложении. 
        Если вы не делали этого, просто игнорируйте данное письмо.</p>
        <center>Код подтверждения: <b>{argument}</b></center>";
    }
}