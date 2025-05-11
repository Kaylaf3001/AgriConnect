using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
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

        // GET: Add product form
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: Add product
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsModel model)
        {
            if (ModelState.IsValid)
            {
                var farmerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (farmerIdClaim == null)
                {
                    ModelState.AddModelError("", "Farmer is not logged in.");
                    return View(model);
                }

                var farmerId = int.Parse(farmerIdClaim);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.FarmerID == farmerId);

                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer not found.");
                    return View(model);
                }

                model.FarmerID = farmer.FarmerID;
                model.Farmer = farmer;

                _context.Products.Add(model);
                await _context.SaveChangesAsync();

                TempData["ProductAdded"] = true;
                return RedirectToAction("AddProduct");
            }

            return View(model);
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
    }
}
