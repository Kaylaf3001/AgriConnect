using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

//----------------------------------------------------------------------------------------------------------------------
// This is the base controller for the application.
// This method checks if the user is authenticated and has the required claims.
// If not, it redirects the user to the login page.
//----------------------------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Controllers
{
    public class BaseController : Controller
    {
        //----------------------------------------------------------------------------------------------------------------------
        //If the user ever tries to access a page without the Id being properly stored they will be redirected to the login page
        //----------------------------------------------------------------------------------------------------------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
            {
                context.Result = RedirectToAction("Login", "Login");
            }
            base.OnActionExecuting(context);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------End--Of--File-------------------------------------------------------------------------
