using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.ViewModels;
using Part2_FarmerApplication.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Part2_FarmerApplication.Controllers
{
    public class FarmerController : Controller
    {
        private readonly IFarmerRepository _farmerRepo;
        private readonly IProductRepository _productRepo;

        public FarmerController(IFarmerRepository farmerRepo, IProductRepository productRepo)
        {
            _farmerRepo = farmerRepo;
            _productRepo = productRepo;
        }

        // Farmer Dashboard
        public async Task<IActionResult> FarmerDashboard()
        {
            var farmerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (farmerIdClaim == null)
                return Unauthorized();

            int farmerId = int.Parse(farmerIdClaim);
            var farmer = await _farmerRepo.GetFarmerByIdAsync(farmerId);
            if (farmer == null)
                return NotFound();

            var products = await _productRepo.GetProductsByFarmerIdAsync(farmerId);
            var recentProducts = products
                .OrderByDescending(p => p.ProductionDate)
                .Take(5)
                .Select(p => new FarmersProductsViewModel(p, farmer))
                .ToList();

            var model = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                TotalProducts = products.Count,
                RecentProducts = recentProducts
            };

            return View(model);
        }

        // View all products for the logged-in farmer
        public async Task<IActionResult> ViewProducts()
        {
            var farmerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (farmerIdClaim == null)
                return Unauthorized();

            int farmerId = int.Parse(farmerIdClaim);
            var products = await _productRepo.GetProductsByFarmerIdAsync(farmerId);

            return View(products);
        }

        // List all products with farmer info (for admin or farmer)
        [HttpGet]
        public IActionResult FarmersProducts()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _productRepo.GetAllProductsWithFarmers();

            if (userRole == "Farmer" && int.TryParse(userId, out int farmerId))
            {
                query = query.Where(p => p.FarmerID == farmerId);
            }

            var products = query.Select(p => new FarmersProductsViewModel
            {
                ProductID = p.ProductID,
                ProductName = p.Name,
                Category = p.Category,
                ProductionDate = p.ProductionDate,
                FarmerFirstName = p.Farmer.FirstName,
                FarmerLastName = p.Farmer.LastName,
                ImagePath = !string.IsNullOrEmpty(p.ImagePath) ? p.ImagePath : "/FarmersProductsImages/placeholder.jpg"
            }).ToList();

            return View(products);
        }
    }
}
