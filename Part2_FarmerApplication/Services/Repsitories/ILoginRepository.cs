using Part2_FarmerApplication.Models;
using Part2_FarmerApplication.ViewModels;

//-------------------------------------------------------------------------------------------------
// This interface defines the contract for the Login repository.
// It includes methods for retrieving admin and farmer details based on email and password.
// It is used to abstract the data access layer from the business logic layer.
//-------------------------------------------------------------------------------------------------

public interface ILoginRepository
{
    Task<AdminModel> GetAdminByEmailAndPasswordAsync(string email, string password);
    Task<FarmerModel> GetFarmerByEmailAndPasswordAsync(string email, string password);
}
//-----------------------------------End--Of--File----------------------------------------------------------