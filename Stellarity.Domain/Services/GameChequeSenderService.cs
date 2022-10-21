using Stellarity.Domain.Email;
using Stellarity.Domain.Models;

namespace Stellarity.Domain.Services;

public class GameChequeSenderService
{
    private readonly MailingService _mailingService;

    public GameChequeSenderService(MailingService mailingService)
    {
        _mailingService = mailingService;
    }

    public async Task SendAsync(string email, Key key)
    {
        var cheque = new PurchaseCheque(key);
        await _mailingService.SendGameCheque(email, cheque);
    }
}