using Microsoft.EntityFrameworkCore;
using Web_LW.Models;

var builder = WebApplication.CreateBuilder(args);

// Sessions
builder.Services.AddSession();

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // IMPORTANT !
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Route par défaut → Categories
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categories}/{action=Index}/{id?}");

app.Run();
