using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Part2_FarmerApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminID", "Email", "Name", "Password", "Role" },
                values: new object[] { 1, "admin@example.com", "Admin User", "admin123", "Admin" });

            migrationBuilder.InsertData(
                table: "Farmers",
                columns: new[] { "FarmerID", "AdminID", "City", "ContactNumber", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { 1, 1, "GreenVille", 1234567890, "farmer@example.com", "John", "Doe", "farmer123", "Farmer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Farmers",
                keyColumn: "FarmerID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminID",
                keyValue: 1);
        }
    }
}
