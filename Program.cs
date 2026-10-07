using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using RutaLibreMvc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Conexión a la base de datos SQL Server "RutaLibreDB"
// (la cadena de conexión está en appsettings.json -> ConnectionStrings:DefaultConnection)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Verificación de conexión al iniciar: las tablas NO se crean desde el programa,
// se crean con los scripts SQL de la carpeta "Base de datos".
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        if (context.Database.CanConnect())
            logger.LogInformation("Conectado a la base de datos RutaLibreDB.");
        else
            logger.LogError("No se pudo conectar a RutaLibreDB. Revise la cadena de conexión en appsettings.json y que la base exista.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al conectar con SQL Server. Revise la cadena de conexión en appsettings.json.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Formato numérico con punto decimal (30.00) para que los precios del formulario
// se lean igual en cualquier PC, sin importar la configuración regional de Windows.
var cultura = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehiculos}/{action=Index}/{id?}");

app.Run();
