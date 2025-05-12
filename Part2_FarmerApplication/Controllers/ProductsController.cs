using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Part2_FarmerApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AddProduct()
    {
        // Predefined list of categories
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
        public async Task<IActionResult> AddProduct(ProductsModel product, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the logged-in farmer's ID from claims
                var farmerIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (farmerIdClaim == null)
                {
                    ModelState.AddModelError("", "Farmer is not logged in.");
                    return RedirectToAction("AddProduct", "Products");
                }

                var farmerId = int.Parse(farmerIdClaim.Value);

                // Check if the farmer exists in the database
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.FarmerID == farmerId);
                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer not found.");
                    return RedirectToAction("AddProduct", "Products");
                }

                // Handle image upload
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "FarmersProductsImages");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    product.ImagePath = "/FarmersProductsImages/" + uniqueFileName;
                }

                // Assign the FarmerID and Farmer navigation property to the product
                product.FarmerID = farmer.FarmerID;
                product.Farmer = farmer;

                try
                {
                    // Add the product to the database
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["ProductAdded"] = true;
                    return RedirectToAction("AddProduct", "Products");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error adding product: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the product.");
                }
            }

            return RedirectToAction("AddProduct", "Products");
        }
    }
}