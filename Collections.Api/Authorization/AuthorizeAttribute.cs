using Collections.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Collections.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        var onlyAdmin = context.ActionDescriptor.EndpointMetadata.OfType<OnlyAdminAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }
        var user = (User?)context.HttpContext.Items["User"];
        if (user == null || (onlyAdmin && !user.Admin))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
