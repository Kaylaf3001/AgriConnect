using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.ViewModels;

//-------------------------------------------------------------------------------------------------
// This interface defines the contract for the Farmer repository.
// It includes methods for retrieving, adding, and filtering farmers and their products.
// It is used to abstract the data access layer from the business logic layer.
//-------------------------------------------------------------------------------------------------

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
    Task<List<FarmersProductsViewModel>> GetRecentProductsViewModelByFarmerAsync(int farmerId, int count);
    Task<List<FarmersProductsViewModel>> GetFarmersProductsViewModelsAsync(string userRole, int? farmerId);
    Task<List<FarmersProductsViewModel>> FilterFarmersProductsAsync(string? category, string? farmerName, DateTime? startDate, DateTime? endDate);
    Task<List<FarmerModel>> GetFarmersByAdminIdAsync(int adminId);

}

//---------------------------------End--Of--File------------------------------------------------------------
