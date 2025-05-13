using Part2_FarmerApplication.Models;

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
