using AppLogger = Application.Interfaces.ILogger;
using Infrastructure.Data;
using Infrastructure.Logging;
using Application;
using Application.Interfaces;
using Application.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AppLogger, Logger>();
builder.Services.AddScoped<IDatabase, BadDb>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<CreateOrder>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<AppLogger>();
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error global");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal server error");
    }
});

app.MapGet("/health", (AppLogger logger) =>
{
    logger.Log("Health check started");
    return $"ok {DateTime.Now.Ticks}";
});
// Endpoint de creación de orden usando DI
app.MapPost("/orders", async (HttpContext http, CreateOrder useCase) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = await reader.ReadToEndAsync();
    var parts = (body ?? "").Split(',');

    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 && int.TryParse(parts[2], out var q) ? q : 1;
    var price = parts.Length > 3 && decimal.TryParse(parts[3], out var p) ? p : 0.99m;

    var order = useCase.Execute(customer, product, qty, price);
    return Results.Ok(order);
});

app.MapGet("/info", (IDatabase db) => new
{
    sql = (db as BadDb)?.ConnectionString,
    env = Environment.GetEnvironmentVariables(),
    version = "v0.0.1-secure"
});

await app.RunAsync();

public partial class Program
{
    // Constructor protegido para cumplir con la recomendación del analizador
    protected Program() { }
}
