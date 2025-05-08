using Microsoft.AspNetCore.Mvc;

namespace Part2_FarmerApplication.Controllers
{
    public class FarmerController : Controller
    {
        public IActionResult Dashboard()
        {
            // Farmer-specific content
            return View();
        }
    }
}
