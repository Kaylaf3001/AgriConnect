using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;


namespace Part2_FarmerApplication.Controllers
{
    public class ProductsController : Controller
    {

        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsModel product)
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

        // Filter by category/date range
        public IActionResult Filter(string category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);

            var result = query.ToList();
            return View(result);
        }

        
    }
}