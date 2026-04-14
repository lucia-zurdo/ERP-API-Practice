using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using src.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Base de datos ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opciones =>
opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── CORS ──────────────────────────────────────────────────────────────
var origenesPermitidos = builder.Configuration.GetSection("origenesPermitidos").Get<string[]>();

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origenesPermitidos!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("*");
    });
});

// ── Autenticación Auth0 ──────────────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "permissions"
        };
    });

// ── Autorización por permisos (RBAC) ──────────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    void AddPolicy(string permission) =>
        options.AddPolicy(permission, policy =>
            policy.RequireClaim("permissions", permission));

    // Artículos
    AddPolicy("articulos:read");
    AddPolicy("articulos:write");
    AddPolicy("articulos:delete");

    // Clientes
    AddPolicy("clientes:read");
    AddPolicy("clientes:write");
    AddPolicy("clientes:delete");

    // Proveedores
    AddPolicy("proveedores:read");
    AddPolicy("proveedores:write");
    AddPolicy("proveedores:delete");

    // Facturas venta
    AddPolicy("facturas-venta:read");
    AddPolicy("facturas-venta:write");
    AddPolicy("facturas-venta:delete");

    // Facturas compra
    AddPolicy("facturas-compra:read");
    AddPolicy("facturas-compra:write");
    AddPolicy("facturas-compra:delete");
});

var app = builder.Build();

// Permite peticiones desde cualquier origen durante el desarrollo
app.UseCors();

// Redirige automáticamente las peticiones HTTP a HTTPS
app.UseHttpsRedirection();

// Habilita el sistema de autorización
app.UseAuthorization();

// Conecta las rutas HTTP con los métodos de los controllers
app.MapControllers();

// Arranca el servidor y empieza a escuchar peticiones
app.Run();
