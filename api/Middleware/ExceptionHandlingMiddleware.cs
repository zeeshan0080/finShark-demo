using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using finShark_demo.Middleware;
using finShark_demo.Utils;

namespace finShark_demo.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly bool _isDevelopment;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, bool isDevelopment)
    {
        _next = next;
        _logger = logger;
        _isDevelopment = isDevelopment;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            // Handle business logic exceptions as Bad Request (400)
            LogExceptionWithEndpointInfo(context, ex, "Business logic error");
            await HandleExceptionAsync(context, ex, 400);
        }
        catch (ArgumentException ex)
        {
            // Handle argument exceptions as Bad Request (400)
            LogExceptionWithEndpointInfo(context, ex, "Argument error");
            await HandleExceptionAsync(context, ex, 400);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Handle unauthorized exceptions as Unauthorized (401)
            LogExceptionWithEndpointInfo(context, ex, "Authorization error");
            await HandleExceptionAsync(context, ex, 401);
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions as Internal Server Error (500)
            LogExceptionWithEndpointInfo(context, ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex, 500);
        }
    }

    private void LogExceptionWithEndpointInfo(HttpContext context, Exception exception, string errorType)
    {
        // Get endpoint information if available
        var endpoint = context.GetEndpoint();
        var routeData = context.GetRouteData();

        string controller = "UNKNOWN";
        string action = "UNKNOWN";

        if (routeData != null && routeData.Values.ContainsKey("controller"))
        {
            controller = routeData.Values["controller"]?.ToString() ?? "UNKNOWN";
        }

        if (routeData != null && routeData.Values.ContainsKey("action"))
        {
            action = routeData.Values["action"]?.ToString() ?? "UNKNOWN";
        }

        // Log with endpoint information
        _logger.LogError(
            exception,
            "{ErrorType}: {Message}, Controller: {Controller}, Action: {Action}, Path: {Path}, Method: {Method}, Endpoint: {Endpoint}",
            errorType,
            exception.Message,
            controller,
            action,
            context.Request.Path,
            context.Request.Method,
            endpoint?.DisplayName);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        string message = statusCode == 500 && !_isDevelopment
            ? "An unexpected error occurred"
            : exception.Message;

        string? devMessage = _isDevelopment ? exception.ToString() : null;

        var response = ApiResponse<object>.Fail(message, statusCode, devMessage);

        var json = JsonSerializer.Serialize(response, _jsonOptions);
        await context.Response.WriteAsync(json);
    }
    }
}

// Extension method to add the middleware to the pipeline
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder, bool isDevelopment)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>(isDevelopment);
    }
}