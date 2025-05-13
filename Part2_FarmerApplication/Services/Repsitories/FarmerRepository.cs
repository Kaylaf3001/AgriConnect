using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;

public class FarmerRepository : IFarmerRepository
{
    private readonly AppDbContext _context;

    public FarmerRepository(AppDbContext context)
    {
        _context = context;
    }

    public int GetTotalFarmers()
    {
        return _context.Farmers.Count();
    }

    public async Task<FarmerModel?> GetFarmerByIdAsync(int id)
    {
        return await _context.Farmers.FirstOrDefaultAsync(f => f.FarmerID == id);
    }

    public async Task<List<FarmerModel>> GetRecentFarmersAsync(int count)
    {
        return await _context.Farmers
            .OrderByDescending(f => f.FarmerID)
            .Take(count)
            .ToListAsync();
    }

    public async Task<FarmerModel?> GetFarmerByEmailAsync(string email)
    {
        return await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
    }

    public async Task AddFarmerAsync(FarmerModel farmer)
    {
        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();
    }

    public FarmerModel? GetFarmerById(int farmerId)
    {
        return _context.Farmers.FirstOrDefault(f => f.FarmerID == farmerId);
    }

    public async Task<List<ProductsModel>> GetProductsByFarmerAsync(int farmerId)
    {
        return await _context.Products
            .Where(p => p.FarmerID == farmerId)
            .Include(p => p.Farmer)
            .ToListAsync();
    }

    public IQueryable<ProductsModel> GetAllProductsWithFarmers()
    {
        return _context.Products.Include(p => p.Farmer);
    }
}
