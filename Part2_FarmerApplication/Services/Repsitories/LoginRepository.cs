using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.Services;

//-------------------------------------------------------------------------------------------------
// This class implements the ILoginRepository interface.
// It provides methods to retrieve admin and farmer details based on email and password.
// It uses Entity Framework Core to interact with the database.
//-------------------------------------------------------------------------------------------------
public class LoginRepository : ILoginRepository
{
    // The database context used to interact with the database
    private readonly AppDbContext _context;

    //----------------------------------------------------------------------------------------------------------------------
    // Constructor that takes the database context as a parameter
    //----------------------------------------------------------------------------------------------------------------------
    public LoginRepository(AppDbContext context)
    {
        _context = context;
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Method to get admin details by email and password
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<AdminModel> GetAdminByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Admins.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.Password == password);
    }
    //----------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------
    // Method to get farmer details by email and password
    //----------------------------------------------------------------------------------------------------------------------
    public async Task<FarmerModel> GetFarmerByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Farmers
            .FirstOrDefaultAsync(f => f.Email.ToLower() == email.ToLower() && f.Password == password);
    }
    //----------------------------------------------------------------------------------------------------------------------
}
//----------------------------------------------End--Of--File-----------------------------------------------------------------

