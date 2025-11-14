using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finShark_demo.Utils;


public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string? DevMessage { get; set; }

    // Private constructor to enforce factory methods
    private ApiResponse() { }

    /// <summary>
    /// Creates a successful response
    /// </summary>
    public static ApiResponse<T> Success(T? data = default, string message = "Success", int statusCode = 200, string? devMessage = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            DevMessage = devMessage
        };
    }

    /// <summary>
    /// Creates a failure response
    /// </summary>
    public static ApiResponse<T> Fail(string message, int statusCode = 400, string? devMessage = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Data = default,
            DevMessage = devMessage
        };
    }
}