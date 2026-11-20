using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Database Context with SQL Server / LocalDB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Add HttpContextAccessor and HttpClient service for API communication
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IServiceRequestApiService, ServiceRequestApiService>();

// 3. Add MVC and Web API Controllers with Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Initialize Database and seed sample data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating or seeding the database.");
    }
}

// 5. Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 6. Map Controllers (both MVC and Web API)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
