using Part2_FarmerApplication.Models;

//-------------------------------------------------------------------------------------------------
// This interface defines the contract for the Product repository.
// It includes methods for retrieving, adding, and filtering products.
// It is used to abstract the data access layer from the business logic layer.
//-------------------------------------------------------------------------------------------------

public interface IProductRepository
{
    int GetTotalProducts();
    Task<List<ProductsModel>> GetRecentProductsAsync(int count);
    List<string> GetUniqueCategories();
    IQueryable<ProductsModel> GetAllProductsWithFarmers();
    Task AddProductAsync(ProductsModel product);
    Task<ProductsModel?> GetProductByIdAsync(int productId);
    Task<List<ProductsModel>> GetProductsByFarmerIdAsync(int farmerId);
}
//---------------------------------End--Of--File------------------------------------------------------------