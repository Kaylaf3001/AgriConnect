using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Security.Claims;

//------------------------------------------------------------------------------------------------------
// This is the controller for the Login section of the application
// It handles the login and logout actions
// It uses the AppDbContext to access the data
//------------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Controllers
{
    public class LoginController : Controller
    {
        // Here we are using dependency injection to inject the login repository
        private readonly ILoginRepository _loginRepo;

        //------------------------------------------------------------------------------------------------------
        // Constructor for the LoginController
        //------------------------------------------------------------------------------------------------------
        public LoginController(ILoginRepository loginRepo)
        {
            _loginRepo = loginRepo;
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method returns the login page view
        //------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method handles the login form submission
        //-------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email and password are not null or empty
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                    return View(model);
                }

                // Check if the email and password are valid
                var admin = await _loginRepo.GetAdminByEmailAndPasswordAsync(model.Email, model.Password);

                // Check if the admin is null
                if (admin != null)
                {
                    // Create claims for the admin
                    var claims = new List<Claim>
                {
                    // Create claims for the admin
                    new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
                    new Claim(ClaimTypes.Name, admin.Name),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, admin.Role)
                };

                    // Create claims identity for the admin
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("AdminDashboard", "Admin");
                }

                // Check if the email and password are valid for a farmer
                var farmer = await _loginRepo.GetFarmerByEmailAndPasswordAsync(model.Email, model.Password);

                // Check if the farmer is null
                if (farmer != null)
                {
                    // Create claims for the farmer
                    var claims = new List<Claim>
                {
                    // Create claims for the farmer
                    new Claim(ClaimTypes.NameIdentifier, farmer.FarmerID.ToString()),
                    new Claim(ClaimTypes.Name, $"{farmer.FirstName} {farmer.LastName}"),
                    new Claim(ClaimTypes.Email, farmer.Email),
                    new Claim(ClaimTypes.Role, farmer.Role)
                };
                    // Create claims identity for the farmer
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("FarmerDashboard", "Farmer");
                }

                // If the email and password are invalid, add a model error
                ModelState.AddModelError(nameof(model.Password), "Invalid login attempt. Please ensure your password and email are valid.");
            }

            return View(model);
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method handles the logout action
        //-------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
        //------------------------------------------------------------------------------------------------------
    }
}
//----------------------------------End--Of--File----------------------------------------------------------------
