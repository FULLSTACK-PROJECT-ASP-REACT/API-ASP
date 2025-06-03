namespace ApiNetCore.Exceptions;

public class InternalServerException : BaseException
{
    public InternalServerException(string message) 
        : base(500, "INTERNAL_SERVER_ERROR", message)
    {
    }

    public InternalServerException(string message, Exception innerException) 
        : base(500, "INTERNAL_SERVER_ERROR", message, innerException)
    {
    }
}