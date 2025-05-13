using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;

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
    //----------------------------------------------------------------------------------------------------------------------
    //Displays the products that the farmer has created
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<ProductsModel>> GetProductsByFarmerAsync(int farmerId)
    {
        return await _context.Products
            .Where(p => p.FarmerID == farmerId)
            .Include(p => p.Farmer)
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    public IQueryable<ProductsModel> GetAllProductsWithFarmers()
    {
        return _context.Products.Include(p => p.Farmer);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    //Displays the farmers top 5 products that were recently made
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<FarmersProductsViewModel>> GetRecentProductsViewModelByFarmerAsync(int farmerId, int count)
    {
        return await _context.Products
            .Where(p => p.FarmerID == farmerId)
            .OrderByDescending(p => p.ProductionDate)
            .Take(count)
            .Select(p => new FarmersProductsViewModel(p, p.Farmer))
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    //Products displayed for each farmer
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<FarmersProductsViewModel>> GetFarmersProductsViewModelsAsync(string userRole, int? farmerId)
    {
        var query = _context.Products.Include(p => p.Farmer).AsQueryable();

        if (userRole == "Farmer" && farmerId.HasValue)
        {
            query = query.Where(p => p.FarmerID == farmerId.Value);
        }

        return await query.Select(p => new FarmersProductsViewModel
        {
            ProductID = p.ProductID,
            ProductName = p.Name,
            Category = p.Category,
            ProductionDate = p.ProductionDate,
            FarmerFirstName = p.Farmer.FirstName,
            FarmerLastName = p.Farmer.LastName,
            ImagePath = !string.IsNullOrEmpty(p.ImagePath) ? p.ImagePath : "/FarmersProductsImages/placeholder.jpg"
        }).ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------------------------------------
    //Filter the products by category, farmer name, and date range
    //-----------------------------------------------------------------------------------------------------------------------
    public async Task<List<FarmersProductsViewModel>> FilterFarmersProductsAsync(string? category, string? farmerName, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Products.Include(p => p.Farmer).AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (!string.IsNullOrEmpty(farmerName))
            query = query.Where(p => (p.Farmer.FirstName + " " + p.Farmer.LastName).Contains(farmerName));

        if (startDate.HasValue && endDate.HasValue)
            query = query.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);

        return await query.Select(p => new FarmersProductsViewModel
        {
            ProductID = p.ProductID,
            ProductName = p.Name,
            Category = p.Category,
            ProductionDate = p.ProductionDate,
            FarmerFirstName = p.Farmer.FirstName,
            FarmerLastName = p.Farmer.LastName,
            ImagePath = !string.IsNullOrEmpty(p.ImagePath) ? p.ImagePath : "/FarmersProductsImages/placeholder.jpg"
        }).ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------
}
//-----------------------------------End--Of--File-------------------------------------------------------------------------------
