using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;

namespace Part2_FarmerApplication.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public string FarmerName { get; set; }
        public int TotalProducts { get; set; }
        public List<ProductsModel> Products { get; set; }
    }

}
