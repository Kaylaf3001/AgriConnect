using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;

public class LoginRepository : ILoginRepository
{
    private readonly AppDbContext _context;

    public LoginRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AdminModel> GetAdminByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.Password == password);
    }

    public async Task<FarmerModel> GetFarmerByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Farmers
            .FirstOrDefaultAsync(f => f.Email.ToLower() == email.ToLower() && f.Password == password);
    }
}

