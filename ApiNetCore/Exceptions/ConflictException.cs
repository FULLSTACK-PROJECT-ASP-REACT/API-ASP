namespace ApiNetCore.Exceptions;

public class ConflictException : BaseException
{
    public ConflictException(string message) 
        : base(409, "CONFLICT", message)
    {
    }
}