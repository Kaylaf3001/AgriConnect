using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

//---------------------------------------------------------------------------------
// This is the model for the Admin
// It contains the properties that will be used to store the data
// It also contains the navigation property to the FarmerModel
//---------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Models
{
    public class AdminModel
    {
        // Primary Key
        [Key]
        public int AdminID { get; set; }

        // Required properties
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } = "Employee"; // Default value

        // Navigation Property
        public ICollection<FarmerModel> Farmers { get; set; } = new List<FarmerModel>(); // Default empty list

        // Parameterless constructor (required for EF Core)
        public AdminModel() { }

        //---------------------------------------------------------------------------------
        // Constructor to enforce initialization
        //---------------------------------------------------------------------------------
        public AdminModel(string name, string email, string role = "Employee")
        {
            Name = name;
            Email = email;
            Role = role;
            Farmers = new List<FarmerModel>();
        }
        //---------------------------------------------------------------------------------
    }
}
//--------------------------End--Of--File---------------------------------------------------
