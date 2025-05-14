using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;

//----------------------------------------------------------------------------------------------------------------------
//In this file, we define the ProductRepository class that implements the IProductRepository interface.
// This class is responsible for interacting with the database to perform CRUD operations on products.
// It uses Entity Framework Core to access the database and perform operations asynchronously.
// It also includes methods to get the total number of products, get recent products, get unique categories,
//----------------------------------------------------------------------------------------------------------------------

public class ProductRepository : IProductRepository
{
    // This is the database context that will be used to access the data
    private readonly AppDbContext _context;

    //----------------------------------------------------------------------------------------------------------------------
    // Constructor
    //----------------------------------------------------------------------------------------------------------------------
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get the total number of products
    //----------------------------------------------------------------------------------------------------------------------
    public int GetTotalProducts() => _context.Products.Count();
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get the most recent products (5)
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<ProductsModel>> GetRecentProductsAsync(int count)
    {
        return await _context.Products
            .OrderByDescending(p => p.ProductID)
            .Take(count)
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get unique product categories
    //----------------------------------------------------------------------------------------------------------------------
    public List<string> GetUniqueCategories()
    {
        return _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToList();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get all products with their associated farmers
    //----------------------------------------------------------------------------------------------------------------------
    public IQueryable<ProductsModel> GetAllProductsWithFarmers()
    {
        return _context.Products.Include(p => p.Farmer);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Add a new product
    //----------------------------------------------------------------------------------------------------------------------
    public async Task AddProductAsync(ProductsModel product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get product by ID
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<ProductsModel?> GetProductByIdAsync(int productId)
    {
        return await _context.Products
            .Include(p => p.Farmer)
            .FirstOrDefaultAsync(p => p.ProductID == productId);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get products by farmer ID
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<ProductsModel>> GetProductsByFarmerIdAsync(int farmerId)
    {
        return await _context.Products
            .Where(p => p.FarmerID == farmerId)
            .Include(p => p.Farmer)
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------
}
//--------------------------------------------End--Of--File----------------------------------------------------------------------
