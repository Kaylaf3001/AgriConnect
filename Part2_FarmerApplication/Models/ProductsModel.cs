using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Part2_FarmerApplication.Models
{
    public class ProductsModel
    {
        // Primary Key
        [Key]
        public int ProductID { get; set; }

        // Required properties
        public required string Name { get; set; }
        public required string Category { get; set; }
        public DateTime ProductionDate { get; set; }

        // Foreign Key
        public int FarmerID { get; set; }

        // Navigation Property
        public required FarmerModel Farmer { get; set; } // Required navigation property

        // Parameterless constructor (required for EF Core)
        public ProductsModel() { }

        // Constructor to enforce initialization
        public ProductsModel(string name, string category, DateTime productionDate, FarmerModel farmer)
        {
            Name = name;
            Category = category;
            ProductionDate = productionDate;
            Farmer = farmer;
        }
    }
}
