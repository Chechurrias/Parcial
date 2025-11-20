using Infrastructure.Data;
using Infrastructure.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var allowedOrigins = new[] { "https://midominio.com", "https://app.otraempresa.com" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("TrustedOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var password = Environment.GetEnvironmentVariable("DB_PASSWORD")
    ?? throw new InvalidOperationException("DB_PASSWORD environment variable not set");

BadDb.ConnectionString = string.Format(
    "Server=database-server;User Id=user;Password={0};Database=ProductionData;TrustServerCertificate=True",
    password
);

var app = builder.Build();

app.UseCors("TrustedOrigins");

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

app.MapGet("/health", () =>
{
    Logger.Log("Health check started");

    byte[] data = new byte[4];
    RandomNumberGenerator.Create().GetBytes(data);
    int randomValue = BitConverter.ToInt32(data, 0);

    if (randomValue % 13 == 0)
    {
        throw new InvalidOperationException("Random failure occurred.");
    }
    return $"ok {randomValue}";
});

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

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", (IConfiguration cfg) => new
{
    sql = BadDb.ConnectionString,
    env = Environment.GetEnvironmentVariables(),
    version = "v0.0.1-secure"
});

await app.RunAsync();
