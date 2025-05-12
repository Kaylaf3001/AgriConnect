using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Models;

namespace Part2_FarmerApplication.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<FarmerModel> Farmers { get; set; }
        public DbSet<ProductsModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship: Admin ➜ Many Farmers
            modelBuilder.Entity<FarmerModel>()
                .HasOne(f => f.Admin)
                .WithMany(a => a.Farmers)
                .HasForeignKey(f => f.AdminID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Farmer ➜ Many Products
            modelBuilder.Entity<ProductsModel>()
                .HasOne(p => p.Farmer)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FarmerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Admin
            modelBuilder.Entity<AdminModel>().HasData(new AdminModel
            {
                AdminID = 1,
                Name = "Admin User",
                Email = "admin@example.com",
                Password = "admin123",
                Role = "Admin"
            });

            // Seed Farmer (must match structure and NOT use constructor)
            modelBuilder.Entity<FarmerModel>().HasData(new FarmerModel
            {
                FarmerID = 1,
                FirstName = "John",
                LastName = "Doe",
                City = "GreenVille",
                ContactNumber = 1234567890,
                Email = "farmer@example.com",
                Password = "farmer123",
                Role = "Farmer",
                AdminID = 1
            });
        }
    }
 }
