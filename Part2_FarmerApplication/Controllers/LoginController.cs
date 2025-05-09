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
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                    return View(model);
                }

                // Check for Admin login
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email.ToLower() == model.Email.ToLower() && a.Password == model.Password);

                if (admin != null)
                {
                    // Debug to track admin login
                    Console.WriteLine($"Admin {admin.Name} logged in successfully.");

                    // Create claims for the logged-in admin
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()), // Add AdminID as a claim
                        new Claim(ClaimTypes.Name, admin.Name), // Add Admin name as a claim
                        new Claim(ClaimTypes.Email, admin.Email), // Add Email as a claim
                        new Claim(ClaimTypes.Role, admin.Role) // Add role as a claim
                    };

                    // Create the claims identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Create the claims principal
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in the user (this sets the cookie for the session)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Redirect to the Admin Dashboard after login
                    return RedirectToAction("AdminDashboard", "Dashboard");
                }

                // Check for Farmer login
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email.ToLower() == model.Email.ToLower() && f.Password == model.Password);

                if (farmer != null)
                {
                    // Farmer found, authenticate as farmer (similar approach can be done for Farmer if needed)
                    return RedirectToAction("FarmerDashboard", "Dashboard");
                }

                // If no matching user found, return error
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we reach here, something went wrong; return to the login page with errors
            return View(model);
        }
    }
}