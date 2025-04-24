using Microsoft.AspNetCore.Mvc;

namespace Part2_FarmerApplication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

    }
}
