using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sportify_back.Models;
using Microsoft.Extensions.DependencyInjection;
using Sportify_back.Identity;
using Sportify_Back.Areas.Identity.Data;
using Sportify_Back.Models;
using Sportify_Back.Services;
using QuestPDF;

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Agregar explícitamente el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddDbContext<SportifyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuramos Identity con la fábrica de claims personalizada
builder.Services.AddDefaultIdentity<ApplicationUser>
    (options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<SportifyDbContext>()
    .AddClaimsPrincipalFactory<AdditionalUserClaimsPrincipalFactory>(); // Agregamos aca la fábrica de claims aquí

builder.Services.AddControllersWithViews();

// Configurar la autenticación por cookies (para no generar un controller para el login, que seria otra opcion)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
}).AddCookie();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministradorOnly", policy =>
        policy.RequireClaim("Profile", "Administrador")); // Verifica el claim "Profile" para ser "Administrador"
});

// builder.Services.AddScoped<IEntityReportService, IEntityReportService>();//Agregamos el Service de Reporteria
builder.Services.AddScoped<IEntityReportService, EntityReportService>(); // Agregamos el Service de Reporteria

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Agregamos la autenticacion
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");
app.MapRazorPages();


try
{
    
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw;
}

