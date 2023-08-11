﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace LoginMicroservice.Api.Middlewares;

public class GlobalErrorMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;

        var exceptionType = exception.GetType();


        if (exceptionType == typeof(ValidationException))
        {
            status = HttpStatusCode.BadRequest;
            message = exception.Message;
        }
        else if (exceptionType == typeof(UnauthorizedAccessException))
        {
            status = HttpStatusCode.Unauthorized;
            message = exception.Message;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            message = exception.Message;
        }

        var exceptionResult = JsonSerializer.Serialize(message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}