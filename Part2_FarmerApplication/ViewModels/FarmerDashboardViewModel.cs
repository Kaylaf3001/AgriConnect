using Part2_FarmerApplication.Models;
using System.Collections.Generic;

//-------------------------------------------------------------------------------------------------
// This class represents the view model for the farmer dashboard.
// It contains properties for the farmer details, total number of products,
// and a list of recent products.
//------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public FarmerModel Farmer { get; set; }
        public int TotalProducts { get; set; }
        public List<FarmersProductsViewModel> RecentProducts { get; set; } = new List<FarmersProductsViewModel>();

        public FarmerDashboardViewModel() { }

        public FarmerDashboardViewModel(FarmerModel farmer, int totalProducts, List<FarmersProductsViewModel> recentProducts)
        {
            Farmer = farmer;
            TotalProducts = totalProducts;
            RecentProducts = recentProducts;
        }
    }
}
//------------------------------End--Of--File---------------------------------------------------------------