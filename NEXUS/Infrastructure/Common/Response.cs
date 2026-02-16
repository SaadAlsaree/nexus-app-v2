namespace NEXUS.Infrastructure.Common;

public class Response<T>
{
    public bool Succeeded { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<ValidationErrorDetail> Errors { get; set; } = new();

    public Response() { }

    public static Response<T> Success(T data, string message = "Operation succeeded")
    {
        return new Response<T>
        {
            Succeeded = true,
            Data = data,
            Message = message,
            Code = "Success"
        };
    }

    public static Response<T> Success(string message = "Operation succeeded")
    {
        return new Response<T>
        {
            Succeeded = true,
            Message = message,
            Code = "Success"
        };
    }

    public static Response<T> Failure(Error error)
    {
        var response = new Response<T>
        {
            Succeeded = false,
            Message = error.Description,
            Code = error.Code
        };

        if (error is ValidationError validationError)
        {
            response.Errors = validationError.Errors
                .Select(e => new ValidationErrorDetail(e.Code, e.Description))
                .ToList();
        }

        return response;
    }
}

public class Response : Response<object>
{
}

public record ValidationErrorDetail(string Code, string Description);