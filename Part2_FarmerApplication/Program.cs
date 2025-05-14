using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Part2_FarmerApplication.Services;

// This is the main entry point for the application.
var builder = WebApplication.CreateBuilder(args);

// Register DbContext *before* building the app
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MVC services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";       // Redirect to this path if not logged in
        options.AccessDeniedPath = "/Login/Denied"; // Optional: Access denied page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie lifespan
    });


var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Authentication and Authorization (if required, as discussed previously)
app.UseAuthentication(); // This line enables authentication middleware
app.UseAuthorization();  // This line enables authorization middleware

app.UseRouting();

// Configure endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
