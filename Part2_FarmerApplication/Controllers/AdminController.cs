using Microsoft.AspNetCore.Mvc;

namespace Part2_FarmerApplication.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Admin-specific content
            return View();
        }
    }
}
