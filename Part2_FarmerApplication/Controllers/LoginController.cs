using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // Display login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate that the model meets all teh required fields
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                    return View(model);
                }

                // Check for Admin login
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email.ToLower() == model.Email.ToLower() && a.Password == model.Password);

                // Check if the admin exists in the database therefore making a valid login
                if (admin != null)
                {
                    // Create claims for the logged-in admin
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
                        new Claim(ClaimTypes.Name, admin.Name),
                        new Claim(ClaimTypes.Email, admin.Email),
                        new Claim(ClaimTypes.Role, admin.Role)
                    };

                    // Create a claims identity and principal
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in the user with the claims principal
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("AdminDashboard", "Dashboard");
                }

                // Check for Farmer login
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email.ToLower() == model.Email.ToLower() && f.Password == model.Password);

                if (farmer != null)
                {
                    // Create claims for the logged-in farmer
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, farmer.FarmerID.ToString()),
                    new Claim(ClaimTypes.Name, $"{farmer.FirstName} {farmer.LastName}"),
                    new Claim(ClaimTypes.Email, farmer.Email),
                    new Claim(ClaimTypes.Role, farmer.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("FarmerDashboard", "Dashboard");
                }

                // If no matching user found, return error
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // Logout action to sign out the user
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}