using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        // This action method handles errors and returns an error view with the request ID
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
