namespace Stellarity.Domain.Abstractions;

public abstract class Result
{
    protected Result(string? errorMessage) => ErrorMessage = errorMessage;

    public string? ErrorMessage { get; }

    public abstract bool IsSuccessful { get; }
}