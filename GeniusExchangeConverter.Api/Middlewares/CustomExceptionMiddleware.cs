using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GeniusExchangeConverter.Api.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly ILogger<CustomExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _next = next;
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            context,
            context.Response.StatusCode,
            "An error occurred while processing your request.",
            detail: exception.Message,
            instance: context.Request.Path
        );

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        await context.Response.WriteAsync(json);
    }
}