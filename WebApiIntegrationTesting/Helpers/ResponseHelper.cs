using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApiIntegrationTesting.Helpers
{
    public class ResponseHelper
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ResponseHelper(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public static IActionResult SuccessResponse<T>(
            T? data,
            string? message = "Success",
            string? code = null,
            HttpStatusCode status = HttpStatusCode.OK
        )
        {
            var response = new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Code = code ?? ((int)status).ToString(),
            };

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };

        }
        public static IActionResult ErrorResponse<T>(
            T? data,
            string? message = "Error", 
            string? code = null, 
            HttpStatusCode status = HttpStatusCode.BadRequest, 
            Exception? ex = null
        )
        {
            var includeStackTrace = IsDevelopmentMode() && ex != null;
            var stackTrace = includeStackTrace ? ex?.StackTrace : null;

            var response = new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Code = code ?? ((int)status).ToString(),
                Trace = IsDevelopmentMode() ? stackTrace : null
            };

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        private static bool IsDevelopmentMode()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return environment == "Development";
        }
    }
}
