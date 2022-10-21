namespace Stellarity.Domain.Models;

public class PurchaseCheque
{
    public const decimal VatPercent = 20;

    public string KeyValue { get; }
    public DateTime PurchaseDate { get; }
    public string Title { get; }
    public decimal Total { get; }
    public decimal VatFromTotal { get; }
    public string BuyerEmail { get; }

    public PurchaseCheque(Key key)
    {
        if (key is { IsPurchased: false })
            throw new InvalidOperationException("Can't create cheque for not purchased key");

        KeyValue = key.Value;
        PurchaseDate = key.PurchaseDate!.Value;
        Title = key.Game.Title;
        Total = key.Game.Cost;
        VatFromTotal = key.Game.Cost * (VatPercent / 100);
        BuyerEmail = key.Buyer!.Email;
    }
}