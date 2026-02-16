using FluentValidation; // تأكد من وجود هذا الـ using
using Microsoft.AspNetCore.Diagnostics;
using NEXUS.Infrastructure.Common; // حيث يوجد Error و Response

namespace NEXUS.Extensions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // التعامل مع استثناء التحقق
        if (exception is ValidationException validationEx)
        {
            // لا حاجة لطباعة Error Log في التحقق، Info يكفي
            _logger.LogInformation("Validation failed: {Message}", validationEx.Message);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            var errors = validationEx.Errors
                .Select(e => new Error("Validation", e.ErrorMessage, ErrorType.Validation)) // عدل هذا السطر حسب كلاس Error لديك
                .ToArray();

            var response = new ValidationError(errors);

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }

        // التعامل مع باقي الأخطاء (500)
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new { message = "An internal error occurred." }, cancellationToken);

        return true;
    }
}