using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductsModel product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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