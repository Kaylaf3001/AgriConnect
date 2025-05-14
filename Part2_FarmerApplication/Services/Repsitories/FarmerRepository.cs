using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;
using Part2_FarmerApplication.ViewModels;

//----------------------------------------------------------------------------------------------------------------------
// In this class, we are implementing the IFarmerRepository interface.
// This class is responsible for interacting with the database to perform CRUD operations on farmers.
// It uses Entity Framework Core to access the database and perform operations asynchronously.
// It also includes methods to get the total number of farmers, get recent farmers, get farmer by email or Id,
//----------------------------------------------------------------------------------------------------------------------

public class FarmerRepository : IFarmerRepository
{
    // This is the database context that will be used to access the data
    private readonly AppDbContext _context;

    //----------------------------------------------------------------------------------------------------------------------
    // Constructor
    //----------------------------------------------------------------------------------------------------------------------
    public FarmerRepository(AppDbContext context)
    {
        _context = context;
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get the total number of farmers
    //----------------------------------------------------------------------------------------------------------------------
    public int GetTotalFarmers() => _context.Farmers.Count();
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get Farmer by ID
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<FarmerModel?> GetFarmerByIdAsync(int id)
    {
        return await _context.Farmers.FirstOrDefaultAsync(f => f.FarmerID == id);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get the most recent farmers (5)
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<FarmerModel>> GetRecentFarmersAsync(int count)
    {
        return await _context.Farmers
            .OrderByDescending(f => f.FarmerID)
            .Take(count)
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Get Farmer by Email
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<FarmerModel?> GetFarmerByEmailAsync(string email)
    {
        return await _context.Farmers.FirstOrDefaultAsync(f => f.Email == email);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Here an admin can create a new farmer
    //----------------------------------------------------------------------------------------------------------------------
    public async Task AddFarmerAsync(FarmerModel farmer)
    {
        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    //Displays the farmer by ID
    //----------------------------------------------------------------------------------------------------------------------
    public FarmerModel? GetFarmerById(int farmerId)
    {
        return _context.Farmers.FirstOrDefault(f => f.FarmerID == farmerId);
    }
    //----------------------------------------------------------------------------------------------------------------------

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

        // If the user is a farmer, filter by their ID
        if (userRole == "Farmer" && farmerId.HasValue)
        {
            query = query.Where(p => p.FarmerID == farmerId.Value);
        }

        // If the user is an admin, show all products
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

        // If a category is provided, filter by category
        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        // If a farmer name is provided, filter by farmer name
        if (!string.IsNullOrEmpty(farmerName))
            query = query.Where(p => (p.Farmer.FirstName.ToLower() + " " + p.Farmer.LastName.ToLower()).Contains(farmerName.ToLower()));

        // If a date range is provided, filter by production date
        if (startDate.HasValue && endDate.HasValue)
            query = query.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);

        // If no filters are applied, return all products
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

    //----------------------------------------------------------------------------------------------------------------------
    // Get farmers by admin ID
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<List<FarmerModel>> GetFarmersByAdminIdAsync(int adminId)
    {
        return await _context.Farmers
            .Where(f => f.AdminID == adminId)
            .ToListAsync();
    }
    //----------------------------------------------------------------------------------------------------------------------
}
//-----------------------------------End--Of--File-------------------------------------------------------------------------------
