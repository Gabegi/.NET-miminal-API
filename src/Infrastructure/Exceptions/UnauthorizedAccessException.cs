namespace Infrastructure.Exceptions;

/// <summary>
/// Exception thrown when user is not authorized to perform an action.
/// </summary>
public class UnauthorizedAccessException : ApplicationException
{
    public UnauthorizedAccessException(string message = "User is not authorized to perform this action.")
        : base(message, "UNAUTHORIZED_ACCESS")
    {
    }
}
