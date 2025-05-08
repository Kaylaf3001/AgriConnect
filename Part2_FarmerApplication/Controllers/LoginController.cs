using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;

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

        // Handle login logic
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
                var admin = await _context.Admins
    .FirstOrDefaultAsync(a => a.Email.ToLower() == model.Email.ToLower() && a.Password == model.Password);


                if (admin != null)
                {
                    //Debug to track admin login
                    Console.WriteLine($"Admin {admin.Name} logged in successfully.");
                    return RedirectToAction("Dashboard", "AdminDashboard");
                }

                // Check for Farmer login
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email.ToLower() == model.Email.ToLower() && f.Password == model.Password);

                if (farmer != null)
                {
                    // Farmer found, authenticate as farmer
                    // Set session or cookies for Farmer
                    return RedirectToAction("Dashboard", "FarmerDashboard");
                }

                // If no matching user found, return error
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we reach here, something went wrong; return to the login page with errors
            return View(model);
        }


    }
}