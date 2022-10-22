namespace Stellarity.Domain.Email.MailMessages;

public class ChangePasswordConfirmationMail : MailTemplate<int>
{
    private const string ConstSubject = "Смена пароля";
    private const string ConstHeader = "<center><h1>Смена пароля</h1></center>";

    public ChangePasswordConfirmationMail() : base(ConstSubject, ConstHeader)
    {
    }

    protected override string CreateMainText(int argument)
    {
        return @$"<p>Здравствуйте, <b>{To.Name}</b>. 
        Вы получили это сообщение, потому что мы получили от Вас запрос на смену пароля.<br/>
        Если вы не делали этого, просто игнорируйте данное письмо.<br/>
        <center>Код подтверждения: <b>{argument}</b></center>";
    }
}