using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Part2_FarmerApplication.Controllers
{
    public class ProductsController : Controller
    {

        //These are the repositories that will be used to access the data
        private readonly IProductRepository _productRepo;
        private readonly IFarmerRepository _farmerRepo;

        //----------------------------------------------------------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------------------------------------------------------
        public ProductsController(IProductRepository productRepo, IFarmerRepository farmerRepo)
        {
            _productRepo = productRepo;
            _farmerRepo = farmerRepo;
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Gets the products categories
        //----------------------------------------------------------------------------------------------------------------------
        public IActionResult AddProduct()
        {
            //The Categories available for the farmer to choose from
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
        //-----------------------------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------------------------
        // Here a farmer can add a new product
        //-----------------------------------------------------------------------------------------------------------------------
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
        //-----------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------End--Of--File-------------------------------------------------------------
