using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class AdminController : Controller
    {
        //AppDbContext is a class that represents the database context for the application.
        private readonly AppDbContext _context;

        //----------------------------------------------------------------------------------------------------------------------
        // Constructor to initialize the AppDbContext
        //----------------------------------------------------------------------------------------------------------------------
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Admin Dashboard
        //----------------------------------------------------------------------------------------------------------------------
        public IActionResult Dashboard()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Create Farmer form
        //----------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult CreateFarmer()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Here an admin can create a new farmer
        //----------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateFarmer(FarmerModel farmer)
        {
            //Check if the model state is valid
            if (ModelState.IsValid)
            {
                //Check if email already exists
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

                // Check if the save was successful by querying the database
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
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Filter by category/date range
        //----------------------------------------------------------------------------------------------------------------------
        public IActionResult FilterFarmers(string category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);
       
            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);

            // If the user is a farmer, filter products by their FarmerID
            var result = query
                .Select(p => new Part2_FarmerApplication.ViewModels.FarmersProductsViewModel
                {
                    ProductName = p.Name,
                    Category = p.Category,
                    ProductionDate = p.ProductionDate,
                    FarmerFirstName = p.Farmer.FirstName,
                    FarmerLastName = p.Farmer.LastName
                })
                .ToList();

            return View(result);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
//------------------------------------------------End-of-File-------------------------------------------------------------------
