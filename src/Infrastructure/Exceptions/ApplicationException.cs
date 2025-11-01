namespace Infrastructure.Exceptions;

/// <summary>
/// Base application exception for domain-specific errors.
/// </summary>
public class ApplicationException : Exception
{
    public string Code { get; }
    public object? AdditionalData { get; }

    public ApplicationException(string message, string code = "APPLICATION_ERROR", object? data = null)
        : base(message)
    {
        Code = code;
        AdditionalData = data;
    }
}
