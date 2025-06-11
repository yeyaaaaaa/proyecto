using System.Globalization;
using Proyecto.Data;
using Proyecto.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization; // <-- Agrega esto

var builder = WebApplication.CreateBuilder(args);

// Configuración de SmtpSettings (asegúrate de tener el bloque SmtpSettings en appsettings.json)
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Registrar EmailService pasando la configuración concreta de SmtpSettings
builder.Services.AddTransient<Proyecto.Services.EmailService>(sp =>
{
    var smtpSettings = sp.GetRequiredService<IOptions<SmtpSettings>>().Value;
    return new Proyecto.Services.EmailService(smtpSettings);
});

// Parte de la conexion a la base de datos.
builder.Services.AddDbContext<ProyectoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// --- LOCALIZACIÓN ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Configuración de autenticación.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
    });

var app = builder.Build();

// --- LOCALIZACIÓN: configuración de culturas soportadas ---
var supportedCultures = new[] { "es", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("es")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Autenticación y autorización.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();