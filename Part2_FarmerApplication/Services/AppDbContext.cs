using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;

//----------------------------------------------------------------------------------------------------------------------
// This class represents the database context for the application.
// It inherits from DbContext and is responsible for configuring the database connection,
// defining the DbSets for the models, and configuring the relationships between the models.
// It also seeds the database with initial data for Admin and Farmer models.
//----------------------------------------------------------------------------------------------------------------------

namespace Part2_FarmerApplication.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets for the models
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<FarmerModel> Farmers { get; set; }
        public DbSet<ProductsModel> Products { get; set; }

        // Configuring the model relationships and seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //----------------------------------------------------------------------------------------------------------------------
            // Relationship: Admin ➜ Many Farmers
            //----------------------------------------------------------------------------------------------------------------------
            modelBuilder.Entity<FarmerModel>()
                .HasOne(f => f.Admin)
                .WithMany(a => a.Farmers)
                .HasForeignKey(f => f.AdminID)
                .OnDelete(DeleteBehavior.Cascade);
            //----------------------------------------------------------------------------------------------------------------------

            //----------------------------------------------------------------------------------------------------------------------
            // Relationship: Farmer ➜ Many Products
            //----------------------------------------------------------------------------------------------------------------------
            modelBuilder.Entity<ProductsModel>()
                .HasOne(p => p.Farmer)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FarmerID)
                .OnDelete(DeleteBehavior.Cascade);
            //----------------------------------------------------------------------------------------------------------------------

            //----------------------------------------------------------------------------------------------------------------------
            // Prepopulate Admin data
            //----------------------------------------------------------------------------------------------------------------------
            modelBuilder.Entity<AdminModel>().HasData(new AdminModel
            {
                AdminID = 1,
                Name = "Admin User",
                Email = "admin@gmail.com",
                Password = "admin123",
                Role = "Admin"
            });
            //----------------------------------------------------------------------------------------------------------------------

            //----------------------------------------------------------------------------------------------------------------------
            //  Prepopulate Farmer data
            //----------------------------------------------------------------------------------------------------------------------
            modelBuilder.Entity<FarmerModel>().HasData(new FarmerModel
            {
                FarmerID = 1,
                FirstName = "Farmer",
                LastName = "Brown",
                City = "Cape Town",
                ContactNumber = 1234567890,
                Email = "Brown@gmail.com",
                Password = "brown123",
                Role = "Farmer",
                AdminID = 1
            });
            //----------------------------------------------------------------------------------------------------------------------
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------End--Of--File----------------------------------------------------------------------
