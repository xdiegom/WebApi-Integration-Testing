using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WebApiIntegrationTesting.Helpers;

public class HandleRequestValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var validationErrors = context.ModelState.ToDictionary(
                entry => GetDisplayName(entry.Key),
                entry => entry!.Value!.Errors.Select(error => error.ErrorMessage).ToList()
            );

            context.Result = ResponseHelper.ErrorResponse<object>(
                    validationErrors,
                    "Validation failed",
                    "ValidationException",
                    HttpStatusCode.UnprocessableEntity
                );
        }
    }

    private string GetDisplayName(string propertyName)
    {
        return new SnakeCaseNamingPolicy().ConvertName(propertyName);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}