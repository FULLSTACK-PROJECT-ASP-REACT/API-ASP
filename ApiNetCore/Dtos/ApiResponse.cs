namespace ApiNetCore.Dtos;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, List<string>>? ValidationErrors { get; set; }
    public DateTime Timestamp { get; set; }
    public double ResponseTimeMs { get; set; }
    public string RequestId { get; set; }

    protected ApiResponse()
    {
        Timestamp = DateTime.UtcNow;
        RequestId = Guid.NewGuid().ToString();
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, string? errorCode = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
    }

    public static ApiResponse<T> ValidationErrorResponse(Dictionary<string, List<string>> validationErrors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Validation errors occurred",
            ErrorCode = "VALIDATION_ERROR",
            ValidationErrors = validationErrors
        };
    }
}

// Para operaciones que no retornan data
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResponse(string message = "Operation completed successfully")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Data = null
        };
    }

    public new static ApiResponse ErrorResponse(string message, string? errorCode = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
    
}