using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Part2_FarmerApplication.Models
{
    public class FarmerModel
    {
        // Primary Key
        [Key]
        public int FarmerID { get; set; }
        // Required properties
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string City { get; set; }
        public required int ContactNumber { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } = "Farmer"; // Default value

        // Foreign Key to Admin
        public int AdminID { get; set; }

        // Navigation Property
        public AdminModel? Admin { get; set; }

        // Navigation Property
        public ICollection<ProductsModel> Products { get; set; } = new List<ProductsModel>();

        // Parameterless constructor (required for EF Core)
        public FarmerModel() { }

        // Constructor to enforce initialization
        public FarmerModel(string firstName, string lastName, string city, int contactNumber, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            ContactNumber = contactNumber;
            Email = email;
            Password = password;
            Role = "Farmer"; // Default value
            Products = new List<ProductsModel>(); // Initialize collection
        }
    }
}
