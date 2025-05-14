using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

//---------------------------------------------------------------------------------
// This filter checks if the user has the required role to access a specific action.
// If the user does not have the required role, they are redirected to the login page.
//---------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Services.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
        // Array of allowed roles
        private readonly string[] _allowedRoles;

        //---------------------------------------------------------------------------------
        // Constructor to initialize the allowed roles
        //---------------------------------------------------------------------------------
        public RoleFilter(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }
        //---------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------
        // This method is called before the action method is executed.
        //---------------------------------------------------------------------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            
            if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
        //---------------------------------------------------------------------------------
    }
}
//-------------------------------End--Of--File-------------------------------------------------
