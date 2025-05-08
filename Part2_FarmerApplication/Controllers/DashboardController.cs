using Microsoft.AspNetCore.Mvc;

namespace Part2_FarmerApplication.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult FarmerDashboard()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
