using Part2_FarmerApplication.Models;

public interface IFarmerRepository
{
    int GetTotalFarmers();
    Task<List<FarmerModel>> GetRecentFarmersAsync(int count);
    Task<FarmerModel> GetFarmerByEmailAsync(string email);
    Task AddFarmerAsync(FarmerModel farmer);
    FarmerModel? GetFarmerById(int farmerId); // used in FarmerDashboard
    Task<List<ProductsModel>> GetProductsByFarmerAsync(int farmerId); // used in ViewProducts and Dashboard
    IQueryable<ProductsModel> GetAllProductsWithFarmers(); // used in FarmersProducts
    Task<FarmerModel?> GetFarmerByIdAsync(int id);
}
