using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.ViewModels;

public interface ILoginRepository
{
    Task<AdminModel> GetAdminByEmailAndPasswordAsync(string email, string password);
    Task<FarmerModel> GetFarmerByEmailAndPasswordAsync(string email, string password);
}