namespace Stellarity.Domain.Abstractions;

public abstract class Result
{
    protected Result(string? errorMessage, int? errorCode)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public int? ErrorCode { get; }
    
    public string? ErrorMessage { get; }

    public abstract bool IsSuccessful { get; }
}