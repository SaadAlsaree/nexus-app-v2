using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Infrastructure.Middlewares;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. هل الخطأ هو خطأ تحقق من Wolverine؟
        if (exception is not ValidationException validationEx)
        {
            return false; // ليس خطأنا، دع النظام يتعامل معه
        }

        // 2. تحويل أخطاء FluentValidation إلى كائنات Error الخاصة بك
        // افترضت هنا أن الكلاس Error لديك يقبل (Code, Message) في الكونستركتر
        // إذا كان مختلفاً، عدل التحويل أدناه
        var errors = validationEx.Errors.Select(failure =>
            new Error(
                $"Validation.{failure.PropertyName}", // الكود: اسم الحقل
                failure.ErrorMessage,                  // الرسالة: تفاصيل الخطأ
                ErrorType.Validation                   // النوع (إن وجد)
            )).ToArray();

        // 3. إنشاء كائن الاستجابة الخاص بك
        var customError = new ValidationError(errors);

        // 4. إعداد الاستجابة (400 Bad Request)
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(customError, cancellationToken);

        return true; // تم التعامل مع الخطأ
    }
}