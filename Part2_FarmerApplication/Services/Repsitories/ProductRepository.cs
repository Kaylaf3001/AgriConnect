using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public int GetTotalProducts() => _context.Products.Count();

    public async Task<List<ProductsModel>> GetRecentProductsAsync(int count)
    {
        return await _context.Products
            .OrderByDescending(p => p.ProductID)
            .Take(count)
            .ToListAsync();
    }

    public List<string> GetUniqueCategories()
    {
        return _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToList();
    }

    public IQueryable<ProductsModel> GetAllProductsWithFarmers()
    {
        return _context.Products.Include(p => p.Farmer);
    }

    public async Task AddProductAsync(ProductsModel product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductsModel?> GetProductByIdAsync(int productId)
    {
        return await _context.Products
            .Include(p => p.Farmer)
            .FirstOrDefaultAsync(p => p.ProductID == productId);
    }

    public async Task<List<ProductsModel>> GetProductsByFarmerIdAsync(int farmerId)
    {
        return await _context.Products
            .Where(p => p.FarmerID == farmerId)
            .Include(p => p.Farmer)
            .ToListAsync();
    }
}
