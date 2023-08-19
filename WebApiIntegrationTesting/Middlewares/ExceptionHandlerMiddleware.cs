using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using WebApiIntegrationTesting.Helpers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WebApiIntegrationTesting.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = ResponseHelper.ErrorResponse(new object { }, status: HttpStatusCode.InternalServerError, ex: exception);

            return response.ExecuteResultAsync(new ActionContext
            {
                HttpContext = context
            });


            //return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
