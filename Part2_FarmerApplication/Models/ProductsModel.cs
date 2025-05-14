using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

//----------------------------------------------------------------------------------------------------------------------
// This class represents a product model in the application.
// It contains properties for the product ID, name, category, production date, and a foreign key to the farmer.
// It also includes a navigation property to the associated farmer and a property for the image path.
//----------------------------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Models
{
    public class ProductsModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        // Primary Key
        [Key]
        public int ProductID { get; set; }

        // Required properties
        public required string Name { get; set; }

        // Restrict category to predefined values
        [Required]
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }

        // Foreign Key
        public int FarmerID { get; set; }

        // Navigation Property
        public FarmerModel? Farmer { get; set; }

        // New property to store the image path
        public string? ImagePath { get; set; }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Parameterless constructor (required for EF Core)
        //----------------------------------------------------------------------------------------------------------------------
        public ProductsModel() { }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------
        // Constructor to enforce initialization
        //----------------------------------------------------------------------------------------------------------------------
        public ProductsModel(string name, string category, DateTime productionDate, FarmerModel farmer, string? imagePath = null)
        {
            Name = name;
            Category = category;
            ProductionDate = productionDate;
            Farmer = farmer;
            ImagePath = imagePath;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
