namespace Stellarity.Domain.Email;

public record Email(string Name, string Address)
{
    public string Name { get; } = Name;
    public string Address { get; } = Address;
}