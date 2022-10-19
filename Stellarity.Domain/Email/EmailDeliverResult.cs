using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Email;

public class EmailDeliverResult : Result
{
    private EmailDeliverResult(string? errorMessage = null)
        : base(errorMessage)
    {
    }

    public override bool IsSuccessful => ErrorMessage is null;

    public static EmailDeliverResult Success() => new();

    public static EmailDeliverResult NotValidEmail() => new("Such email isn't valid");

    public static EmailDeliverResult NotDelivered() => new("Something went wrong. Mail wasn't delivered. Try later");
}