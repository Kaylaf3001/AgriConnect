using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Diagnostics;

namespace Part2_FarmerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FarmersProducts()
        {
            var products = _context.Products
                .Include(p => p.Farmer) // Eagerly load the Farmer navigation property
                .Select(p => new FarmersProductsViewModel
                {
                    ProductID = p.ProductID,
                    ProductName = p.Name,
                    Category = p.Category,
                    ProductionDate = p.ProductionDate,
                    FarmerFirstName = p.Farmer != null ? p.Farmer.FirstName : "Unknown",
                    FarmerLastName = p.Farmer != null ? p.Farmer.LastName : "Unknown"
                })
                .ToList();

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
