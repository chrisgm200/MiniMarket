using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MiniMarketWebApp.Data;
using MiniMarketWebApp.Models;
using MiniMarketWebApp.Utilities;

var builder = WebApplication.CreateBuilder(args);

// ============================
//   Servicios del sistema
// ============================
builder.Services.AddControllersWithViews();

// Conexión a SQL Server
builder.Services.AddDbContext<MiniMarketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MiniMarketConnection")));

// Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// ============================
//   Middleware
// ============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/ErrorGeneral");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Manejo de error 404 personalizado
app.UseStatusCodePagesWithReExecute("/Home/Error404");


// ============================
//   Crear usuarios iniciales
// ============================
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MiniMarketContext>();

    // Necesitas este using para poder usar "Migrate"
    // (ya está añadido arriba)
    ctx.Database.Migrate();

    if (!ctx.Usuarios.Any())
    {
        ctx.Usuarios.AddRange(
            new Usuario
            {
                Nombre = "Administrador",
                Email = "admin@minimarket.local",
                PasswordHash = PasswordHelper.Hash("Admin123!"),
                Rol = "Administrador"
            },
            new Usuario
            {
                Nombre = "Cajero",
                Email = "cajero@minimarket.local",
                PasswordHash = PasswordHelper.Hash("Cajero123!"),
                Rol = "Cajero"
            }
        );

        ctx.SaveChanges();
    }
}

// ============================
//   Rutas
// ============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
