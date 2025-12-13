using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Airline_Management_System__AMS_.Resources;
using XLocalizer;
using XLocalizer.Xml; // If using XML, but strict requirement is RESX. XLocalizer main package handles the abstraction.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddXLocalizer<SharedResource>(ops =>
    {
        ops.ResourcesPath = "Resources";
        ops.AutoAddKeys = false; // RESX is read-only at runtime
        ops.AutoTranslate = false;
    });

// Configure Localization Options
builder.Services.Configure<RequestLocalizationOptions>(ops =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ar")
    };

    ops.DefaultRequestCulture = new RequestCulture("en");
    ops.SupportedCultures = supportedCultures;
    ops.SupportedUICultures = supportedCultures;
    ops.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider()); // Prioritize cookies
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Email sender (if موجود عندك)
builder.Services.AddTransient<IEmailSender, EmailSender>();


var app = builder.Build();

// Apply pending migrations automatically
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Localization Middleware
const string defaultCulture = "en";
var supportedCultures = new[]
{
    new CultureInfo(defaultCulture),
    new CultureInfo("ar")
};
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

// ✅ Important: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
