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
