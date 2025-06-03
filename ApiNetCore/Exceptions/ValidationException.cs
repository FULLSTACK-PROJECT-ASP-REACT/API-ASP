namespace ApiNetCore.Exceptions;

public class ValidationException : BaseException
{
    public Dictionary<string, List<string>> Errors { get; }

    public ValidationException(Dictionary<string, List<string>> errors) 
        : base(400, "VALIDATION_ERROR", "One or more validation errors occurred")
    {
        Errors = errors;
    }

    public ValidationException(string field, string error) 
        : base(400, "VALIDATION_ERROR", $"Validation error in {field}: {error}")
    {
        Errors = new Dictionary<string, List<string>>
        {
            { field, new List<string> { error } }
        };
    }
}