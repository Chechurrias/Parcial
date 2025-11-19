using Infrastructure.Data;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

// Crear builder para la app
var builder = WebApplication.CreateBuilder(args);

// Configurar logging (remover todos los proveedores predeterminados para personalización)
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Agregar consola para debugging básico

// Configurar política CORS de forma explicita y segura
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Establecer la cadena de conexión desde configuración con fallback seguro
BadDb.ConnectionString = builder.Configuration.GetConnectionString("Sql")
    ?? "Server=localhost;Database=master;User Id=sa;Password=SuperSecret123!;TrustServerCertificate=True";

var app = builder.Build();

// Usar CORS con nombre explícito
app.UseCors("AllowAll");

// Middleware global para manejo simple de errores
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Logger.LogError($"Error global: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal server error");
    }
});

// Endpoint health con logging y control de fail aleatorio
app.MapGet("/health", () =>
{
    Logger.Log("Health check started");
    var random = new Random();
    var number = random.Next();
    if (number % 13 == 0)
    {
        throw new Exception("Random failure");
    }
    return $"ok {number}";
});

// Endpoint para crear orden usando el usecase con inyección de dependencias
app.MapPost("/orders", async (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = await reader.ReadToEndAsync();

    var parts = (body ?? "").Split(',');

    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 && int.TryParse(parts[2], out var q) ? q : 1;
    var price = parts.Length > 3 && decimal.TryParse(parts[3], out var p) ? p : 0.99m;

    var uc = new CreateOrderUseCase(new OrderService(), new BadDb(), new Logger());
    var order = uc.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

// Últimos órdenes
app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

// Información de configuración
app.MapGet("/info", (IConfiguration cfg) => new
{
    sql = BadDb.ConnectionString,
    env = Environment.GetEnvironmentVariables(),
    version = "v0.0.1-secure"
});

app.Run();
