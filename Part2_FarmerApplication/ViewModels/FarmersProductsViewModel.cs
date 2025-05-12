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
    }
}