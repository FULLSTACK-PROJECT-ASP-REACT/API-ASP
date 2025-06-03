namespace ApiNetCore.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message) 
        : base(400, "BAD_REQUEST", message)
    {
    }

    public BadRequestException(string message, Exception innerException) 
        : base(400, "BAD_REQUEST", message, innerException)
    {
    }
}