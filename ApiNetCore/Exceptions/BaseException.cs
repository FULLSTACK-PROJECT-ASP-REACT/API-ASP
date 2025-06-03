namespace ApiNetCore.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }

    protected BaseException(int statusCode, string errorCode, string message) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    protected BaseException(int statusCode, string errorCode, string message, Exception innerException) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}