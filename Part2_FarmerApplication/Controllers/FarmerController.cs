using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.ViewModels;
using Part2_FarmerApplication.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Part2_FarmerApplication.Services.Filters;

namespace Part2_FarmerApplication.Controllers
{
    public class FarmerController : BaseController
    {
        //These are the repositories that will be used to access the data
        private readonly IFarmerRepository _farmerRepo;
        private readonly IProductRepository _productRepo;

        //----------------------------------------------------------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------------------------------------------------------
        public FarmerController(IFarmerRepository farmerRepo, IProductRepository productRepo)
        {
            _farmerRepo = farmerRepo;
            _productRepo = productRepo;
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Farmer Dashboard where the total amount of products that the farmer creates is displayed and also the top 5
        // most recent products are displayed.
        //----------------------------------------------------------------------------------------------------------------------
        [RoleFilter("Farmer")]
        public async Task<IActionResult> FarmerDashboard()
        {

            var farmerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (farmerIdClaim == null)
                return Unauthorized();

            int farmerId = int.Parse(farmerIdClaim);
            var farmer = await _farmerRepo.GetFarmerByIdAsync(farmerId);
            if (farmer == null)
                return NotFound();

            var recentProducts = await _farmerRepo.GetRecentProductsViewModelByFarmerAsync(farmerId, 5);
            var totalProducts = await _farmerRepo.GetProductsByFarmerAsync(farmerId);

            var model = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                TotalProducts = totalProducts.Count,
                RecentProducts = recentProducts
            };

            return View(model);
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // List all products with farmer info (for admin or farmer)
        //----------------------------------------------------------------------------------------------------------------------
        [RoleFilter("Farmer")]
        [HttpGet]
        public async Task<IActionResult> FarmersProducts()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int? farmerId = null;
            if (userRole == "Farmer" && int.TryParse(userId, out int id))
            {
                farmerId = id;
            }

            var products = await _farmerRepo.GetFarmersProductsViewModelsAsync(userRole, farmerId);
            return View(products);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------End--Of--File----------------------------------------------------------------------------
