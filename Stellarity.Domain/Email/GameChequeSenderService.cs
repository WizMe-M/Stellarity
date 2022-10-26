using Stellarity.Domain.Models;

namespace Stellarity.Domain.Email;

public class GameChequeSenderService
{
    private readonly MailingService _mailingService;

    public GameChequeSenderService(MailingService mailingService)
    {
        _mailingService = mailingService;
    }

    public async Task<EmailDeliverResult> SendAsync(string email, Key key)
    {
        var cheque = new PurchaseCheque(key);
        return await _mailingService.SendPurchaseChequeAsync(email, cheque);
    }
}