using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Create Farmer form
        [HttpGet]
        public IActionResult CreateFarmer()
        {
            return View();
        }

        // POST: Create Farmer
        [HttpPost]
        public async Task<IActionResult> CreateFarmer(FarmerModel farmer)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check if email already exists
                var existingFarmer = _context.Farmers.FirstOrDefault(f => f.Email == farmer.Email);
                if (existingFarmer != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(farmer);
                }

                // Set default role
                farmer.Role = "Farmer";

                // Get the AdminID from the logged-in user's claims (in your case, the logged-in admin)
                var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieves AdminID from claims

                if (AdminId != null)
                {
                    // Ensure AdminID is correctly assigned
                    farmer.AdminID = int.Parse(AdminId); // Parse AdminID as an integer
                }
                else
                {
                    // Handle the case where no admin is logged in
                    ModelState.AddModelError("", "Admin not logged in.");
                    return View(farmer);
                }

                // Add the farmer to the database
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync(); // Save changes to SQLite

                // Optionally, you can check if the save was successful by querying the database
                var newlyAddedFarmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == farmer.Email);
                if (newlyAddedFarmer != null)
                {
                    // Set TempData for success popup
                    TempData["FarmerAdded"] = true;
                }

                // Redirect to the same view to show success message or navigate elsewhere
                return RedirectToAction("CreateFarmer");
            }

            return View(farmer);
        }
    }
}
