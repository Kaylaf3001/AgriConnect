using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;

namespace Part2_FarmerApplication.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalFarmers { get; set; }
        public int TotalProducts { get; set; }
        public List<FarmerModel> RecentFarmers { get; set; }
        public List<ProductsModel> RecentProducts { get; set; }
    }

}
