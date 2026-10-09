using System.Net;
using System.Text.Json;
using ParkingManagement.Application.Common;
using ParkingManagement.Domain.Exceptions;

namespace ParkingManagement.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";
        List<string>? errors = null;

        switch (exception)
        {
            case BusinessRuleViolationException brEx:
                statusCode = HttpStatusCode.BadRequest;
                message = brEx.Message;
                _logger.LogWarning("Business rule violation: {Message}", brEx.Message);
                break;

            case EntityNotFoundException nfEx:
                statusCode = HttpStatusCode.NotFound;
                message = nfEx.Message;
                _logger.LogWarning("Resource not found: {Message}", nfEx.Message);
                break;

            case DomainException domEx:
                statusCode = HttpStatusCode.BadRequest;
                message = domEx.Message;
                _logger.LogWarning("Domain error: {Message}", domEx.Message);
                break;

            case ArgumentException argEx:
                statusCode = HttpStatusCode.BadRequest;
                message = argEx.Message;
                _logger.LogWarning("Invalid argument: {Message}", argEx.Message);
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                message = "An internal server error occurred while processing your request.";
                break;
        }

        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(message, errors);
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(json);
    }
}
