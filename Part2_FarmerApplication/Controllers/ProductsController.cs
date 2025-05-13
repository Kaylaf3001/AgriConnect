using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IFarmerRepository _farmerRepo;

        public ProductsController(IProductRepository productRepo, IFarmerRepository farmerRepo)
        {
            _productRepo = productRepo;
            _farmerRepo = farmerRepo;
        }

        public IActionResult AddProduct()
        {
            ViewBag.Categories = new List<string>
            {
                "Fruits",
                "Vegetables",
                "Dairy",
                "Grains",
                "Meat",
                "Poultry"
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsModel product)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the logged-in farmer's ID from claims
                var farmerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (farmerIdClaim == null)
                {
                    ModelState.AddModelError("", "Farmer is not logged in.");
                    return RedirectToAction("AddProduct");
                }

                var farmerId = int.Parse(farmerIdClaim.Value);

                // Get the farmer
                var farmer = await _farmerRepo.GetFarmerByIdAsync(farmerId);
                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer not found.");
                    return RedirectToAction("AddProduct");
                }

                // Assign foreign key and default image (if needed)
                product.FarmerID = farmer.FarmerID;
                product.Farmer = farmer;
                product.ImagePath = "/FarmersProductsImages/default.png"; // <-- optional default image

                try
                {
                    await _productRepo.AddProductAsync(product);
                    TempData["ProductAdded"] = true;
                    return RedirectToAction("AddProduct");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding product: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the product.");
                }
            }

            return View(product);
        }
    }
}
