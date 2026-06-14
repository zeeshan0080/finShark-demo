using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Utils;

namespace finShark_demo.Application.Services
{
    public class BaseService
    {
        /// Creates a standardized success response
    /// </summary>
    protected static ApiResponse<T> SuccessResponse<T>(T data, string? message = "Success", int statusCode = 200) =>
        ApiResponse<T>.Success(data, message!, statusCode);

    /// <summary>
    /// Creates a standardized not found response
    /// </summary>
    protected static ApiResponse<T> NotFoundResponse<T>(string message, int statusCode = 404) =>
        ApiResponse<T>.Fail(message, statusCode);

    /// <summary>
    /// Creates a standardized not found response
    /// </summary>
    protected static ApiResponse<T> BadRequestResponse<T>(string message, int statusCode = 400) =>
        ApiResponse<T>.Fail(message, statusCode);

    /// <summary>
    /// Creates a standardized error response
    /// </summary>
    protected static ApiResponse<T> ErrorResponse<T>(string message, int statusCode = 500) =>
        ApiResponse<T>.Fail(message, statusCode);
    }
}