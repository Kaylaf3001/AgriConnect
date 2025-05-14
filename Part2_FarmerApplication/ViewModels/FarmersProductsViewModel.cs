using Part2_FarmerApplication.Models;

//-------------------------------------------------------------------------------------------------
// This class represents the view model for the farmer's products.
// It contains properties for the product ID, name, category, production date,
// farmer's first and last name, and the image path.
//-------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.ViewModels
{
    public class FarmersProductsViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime ProductionDate { get; set; }
        public string FarmerFirstName { get; set; } = string.Empty;
        public string FarmerLastName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public FarmersProductsViewModel() { }

        //----------------------------------------------------------------------------------
        // Fixed constructor name
        //----------------------------------------------------------------------------------
        public FarmersProductsViewModel(ProductsModel product, FarmerModel farmer)
        {
            this.ProductID = product.ProductID;
            this.ProductName = product.Name;
            this.Category = product.Category;
            this.ProductionDate = product.ProductionDate;
            this.FarmerFirstName = farmer.FirstName;
            this.FarmerLastName = farmer.LastName;
            this.ImagePath = product.ImagePath;
        }
        //----------------------------------------------------------------------------------
    }
}
//---------------------------------End--Of--File---------------------------------------------------------------------------------