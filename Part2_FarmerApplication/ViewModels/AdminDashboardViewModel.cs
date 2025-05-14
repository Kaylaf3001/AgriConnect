using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;

//----------------------------------------------------------------------------------------------------------------------
// This class represents the view model for the admin dashboard.
// It contains properties for the total number of farmers, total number of products,
// and lists of recent farmers and products.
//------------------------------------------------------------------------------------------------------------------------

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
//----------------------------------------------------------------------------------------------------------------------
