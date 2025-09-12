using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace API.Filters
{    
    public class ValidateApiRequestAttribute : ActionFilterAttribute
    {
        public int[]? RequiredCredentialsId { get; set; }

        private const string MissingUserIdMessage = "User not found or unauthorized.";
        private const string MissingCredentialsIdMessage = "Missing credentialsId claim.";
        private const string MissingRoleIdMessage = "Missing role claim.";
        private const string InvalidCredentialsIdMessage = "Invalid or unauthorized credentialsId claim.";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            httpContext.Items["apiVersion"] = httpContext.GetRequestedApiVersion()?.ToString() ?? "1";
            httpContext.Items["controllerName"] = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = httpContext.User.FindFirst(ClaimTypes.Role);
            var credentialsClaim = httpContext.User.FindFirst("credentialsId");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId) || userId == 0)
            {
                context.Result = new UnauthorizedObjectResult(MissingUserIdMessage);
                return;
            }

            if (credentialsClaim == null || !int.TryParse(credentialsClaim.Value, out int credentialsId) || credentialsId == 0)
            {
                context.Result = new UnauthorizedObjectResult(MissingCredentialsIdMessage);
                return;
            }

            if (roleClaim == null || !int.TryParse(roleClaim.Value, out int roleId) || roleId == 0)
            {
                context.Result = new UnauthorizedObjectResult(MissingRoleIdMessage);
                return;
            }

            if (RequiredCredentialsId != null && !RequiredCredentialsId.Contains(credentialsId))
            {
                context.Result = new UnauthorizedObjectResult(InvalidCredentialsIdMessage);
                return;
            }

            httpContext.Items["UserId"] = userId;
            httpContext.Items["CredentialsId"] = credentialsId;
            httpContext.Items["RoleId"] = roleId;

            base.OnActionExecuting(context);
        }
    }
}
