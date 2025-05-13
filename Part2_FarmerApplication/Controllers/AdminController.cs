using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.Services.Filters;
using Part2_FarmerApplication.ViewModels;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class AdminController : BaseController
    {
        //These are the repositories that will be used to access the data
        private readonly IFarmerRepository _farmerRepo;
        private readonly IProductRepository _productRepo;

        //----------------------------------------------------------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------------------------------------------------------
        public AdminController(IFarmerRepository farmerRepo, IProductRepository productRepo)
        {
            _farmerRepo = farmerRepo;
            _productRepo = productRepo;
        }
        //----------------------------------------------------------------------------------------------------------------------
        // Admin Dashboard
        //----------------------------------------------------------------------------------------------------------------------
        [RoleFilter("Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalFarmers = _farmerRepo.GetTotalFarmers(),
                TotalProducts = _productRepo.GetTotalProducts(),
                RecentFarmers = await _farmerRepo.GetRecentFarmersAsync(5),
                RecentProducts = await _productRepo.GetRecentProductsAsync(5)
            };

            return View(model);
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
            if (ModelState.IsValid)
            {
                var existingFarmer = await _farmerRepo.GetFarmerByEmailAsync(farmer.Email);
                if (existingFarmer != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(farmer);
                }

                farmer.Role = "Farmer";
                var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (AdminId != null)
                {
                    farmer.AdminID = int.Parse(AdminId);
                }
                else
                {
                    ModelState.AddModelError("", "Admin not logged in.");
                    return View(farmer);
                }

                await _farmerRepo.AddFarmerAsync(farmer);
                TempData["FarmerAdded"] = true;
                return RedirectToAction("CreateFarmer");
            }

            return View(farmer);
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Filter by category/date range
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> FilterFarmers(string category, string farmerName, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.Categories = _productRepo.GetUniqueCategories();

            var result = await _farmerRepo.FilterFarmersProductsAsync(category, farmerName, startDate, endDate);

            return View(result);
        }
        //----------------------------------------------------------------------------------------------------------------------

    }
}
//------------------------------------------------End-of-File-------------------------------------------------------------------
