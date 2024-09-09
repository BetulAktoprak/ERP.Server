using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using TS.Result;

namespace ERP.Server.WebAPI.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = 500;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        Result<string> result = Result<string>.Failure(exception.Message);

        if (exception.GetType() == typeof(ValidationException))
        {
            List<string> errorMessages = JsonSerializer.Deserialize<List<string>>(exception.Message)!;
            result = Result<string>.Failure(errorMessages);
        }

        string message = JsonSerializer.Serialize(result);

        await httpContext.Response.WriteAsync(message, cancellationToken);

        return true;
    }
}
