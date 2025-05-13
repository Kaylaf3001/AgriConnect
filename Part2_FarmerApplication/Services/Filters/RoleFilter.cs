using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Part2_FarmerApplication.Services.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleFilter(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            
            if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
}
