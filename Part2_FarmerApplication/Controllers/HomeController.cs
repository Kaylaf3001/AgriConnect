using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

//------------------------------------------------------------------------------------------------------
// This is the controller for the Home section of the application
// It handles the home page and privacy page
// It uses the AppDbContext to access the data
//------------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Controllers
{
    public class HomeController : Controller
    {
        //Here we are using dependency injection to inject the logger and the database context
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        //------------------------------------------------------------------------------------------------------
        //Constructor for the HomeController
        //------------------------------------------------------------------------------------------------------
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method returns the home page view
        //------------------------------------------------------------------------------------------------------
        public IActionResult Index()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method returns the privacy page view
        //------------------------------------------------------------------------------------------------------
        public IActionResult Privacy()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method handles errors and returns an error view with the request ID
        //------------------------------------------------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        // This action method handles errors and returns an error view with the request ID
        //------------------------------------------------------------------------------------------------------
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //------------------------------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------------------------------
}
//--------------------------------------------End--Of--File------------------------------------------------------
