using Microsoft.AspNetCore.Mvc;
using Part2_FarmerApplication.Models;

namespace Part2_FarmerApplication.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Method to authenticate an Admin
        public AdminModel AuthenticateAdmin(string email, string password)
        {
            return _context.Admins
                .FirstOrDefault(admin => admin.Email == email && admin.Password == password);
        }

        // Method to authenticate a Farmer
        public FarmerModel AuthenticateFarmer(string email, string password)
        {
            return _context.Farmers
                .FirstOrDefault(farmer => farmer.Email == email && farmer.Password == password);
        }
    }
}
