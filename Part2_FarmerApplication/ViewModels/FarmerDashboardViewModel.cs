using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;

namespace Part2_FarmerApplication.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public string FarmerName { get; set; }
        public int TotalProducts { get; set; }
        public List<ProductsModel> Products { get; set; }

        public FarmerDashboardViewModel(FarmerModel farmer)
        {
            this.FarmerName = farmer != null ? $"{farmer.FirstName} {farmer.LastName}" : "Farmer";
            this.TotalProducts = farmer.Products.Count;
            this.Products = farmer.Products.OrderByDescending(p => p.ProductionDate).Take(5).ToList();

        }
    }
}
