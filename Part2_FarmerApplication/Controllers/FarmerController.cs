using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.ViewModels;
using System.Security.Claims;
using Part2_FarmerApplication.Services.Filters;

//----------------------------------------------------------------------------------------------------------------------
// This is the controller for the Farmer section of the application
// It handles the farmer dashboard and listing all products with farmer info
// It uses the FarmerRepository and ProductRepository to access the data
// It also uses the FarmerDashboardViewModel to pass data to the view
//----------------------------------------------------------------------------------------------------------------------

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

            // Check if the farmerIdClaim is null or empty
            if (farmerIdClaim == null)
                return Unauthorized();

            // Parse the farmerIdClaim to an integer
            int farmerId = int.Parse(farmerIdClaim);

            var farmer = await _farmerRepo.GetFarmerByIdAsync(farmerId);

            // Check if the farmer is null
            if (farmer == null)
                return NotFound();

            // Get the recent products and total products for the farmer
            var recentProducts = await _farmerRepo.GetRecentProductsViewModelByFarmerAsync(farmerId, 5);
            var totalProducts = await _farmerRepo.GetProductsByFarmerAsync(farmerId);

            // Check if the recent products or total products are null
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

            // Get the user role and ID from the claims
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user role is null or empty
            int? farmerId = null;
            if (userRole == "Farmer" && int.TryParse(userId, out int id))
            {
                farmerId = id;
            }

            // Get the products for the farmer or all products if the user is an admin
            var products = await _farmerRepo.GetFarmersProductsViewModelsAsync(userRole, farmerId);
            return View(products);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------End--Of--File----------------------------------------------------------------------------
