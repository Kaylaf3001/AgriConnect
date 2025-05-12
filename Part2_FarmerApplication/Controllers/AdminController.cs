using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
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
        public IActionResult AdminDashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalFarmers = _context.Farmers.Count(),
                TotalProducts = _context.Products.Count(),
                RecentFarmers = _context.Farmers
                    .OrderByDescending(f => f.FarmerID)
                    .Take(5)
                    .ToList(),
                RecentProducts = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Take(5)
                    .ToList()
            };

            // Debugging: Log the data
            Console.WriteLine($"Total Farmers: {model.TotalFarmers}");
            Console.WriteLine($"Total Products: {model.TotalProducts}");
            Console.WriteLine($"Recent Farmers: {string.Join(", ", model.RecentFarmers.Select(f => f.FirstName))}");
            Console.WriteLine($"Recent Products: {string.Join(", ", model.RecentProducts.Select(p => p.Name))}");

            return View(model); // Just use "AdminDashboard.cshtml" directly
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
        public IActionResult FilterFarmers(string category, string farmerName, DateTime? startDate, DateTime? endDate)
        {
            // Retrieve the list of unique categories for the dropdown
            ViewBag.Categories = _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            // Filter by category
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            // Filter by farmer's name
            if (!string.IsNullOrEmpty(farmerName))
                query = query.Where(p => (p.Farmer.FirstName + " " + p.Farmer.LastName).Contains(farmerName));

            // Filter by date range
            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);

            // Map the filtered products to the view model
            var result = query
                .Select(p => new Part2_FarmerApplication.ViewModels.FarmersProductsViewModel
                {
                    ProductName = p.Name,
                    Category = p.Category,
                    ProductionDate = p.ProductionDate,
                    FarmerFirstName = p.Farmer.FirstName,
                    FarmerLastName = p.Farmer.LastName,
                    ImagePath = !string.IsNullOrEmpty(p.ImagePath) ? p.ImagePath : "/FarmersProductsImages/placeholder.jpg"
                })
                .ToList();

            return View(result);
        }
        //----------------------------------------------------------------------------------------------------------------------

    }
}
//------------------------------------------------End-of-File-------------------------------------------------------------------
