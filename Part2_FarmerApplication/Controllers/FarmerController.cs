using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Part2_FarmerApplication.Controllers
{
    public class FarmerController : Controller
    {
        private readonly AppDbContext _context;

        public FarmerController(AppDbContext context)
        {
            _context = context;
        }

        
        // Optional: View products created by this farmer
        public async Task<IActionResult> ViewProducts()
        {
            var farmerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (farmerIdClaim == null)
                return Unauthorized();

            var farmerId = int.Parse(farmerIdClaim);

            var products = await _context.Products
                .Where(p => p.FarmerID == farmerId)
                .Include(p => p.Farmer)
                .ToListAsync();

            return View(products);
        }

        /// This action method retrieves all products along with their associated farmers
        [HttpGet]
        public IActionResult FarmersProducts()
        {
            // Retrieve the logged-in user's role
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<ProductsModel> query = _context.Products.Include(p => p.Farmer);

            if (userRole == "Farmer" && int.TryParse(userId, out int farmerId))
            {
                // If the user is a farmer, filter products by their FarmerID
                query = query.Where(p => p.FarmerID == farmerId);
            }

            // Map the filtered products to the view model
            var products = query.Select(p => new FarmersProductsViewModel
            {
                ProductID = p.ProductID,
                ProductName = p.Name,
                Category = p.Category,
                ProductionDate = p.ProductionDate,
                FarmerFirstName = p.Farmer != null ? p.Farmer.FirstName : "Unknown",
                FarmerLastName = p.Farmer != null ? p.Farmer.LastName : "Unknown",
                ImagePath = !string.IsNullOrEmpty(p.ImagePath) ? p.ImagePath : "/FarmersProductsImages/placeholder.jpg" // Default image
            }).ToList();

            return View(products);
        }
    }
}
