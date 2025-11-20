using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using Application.Interfaces;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Quitar el registro real de IDatabase
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDatabase));
            if (descriptor != null)
                services.Remove(descriptor);

            // Agregar dummy para integración
            services.AddScoped<IDatabase, DummyDatabase>();
        });
    }
}

public class ProgramTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    public ProgramTests(CustomWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_Endpoint_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("ok", content);
    }

    [Fact]
    public async Task Orders_Endpoint_ReturnsCreatedOrder()
    {
        var content = new StringContent("Demo,TestItem,2,10", System.Text.Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync("/orders", content);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Demo", result);
        Assert.Contains("TestItem", result);
    }

    [Fact]
    public async Task Info_Endpoint_ReturnsVersionAndConnectionInfo()
    {
        var response = await _client.GetAsync("/info");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        Assert.Contains("v0.0.1-secure", json);
        Assert.True(
        json.Contains("Server=localhost") || 
        json.Contains("\"sql\":null") || 
        json.Contains("Dummy"),
        $"La respuesta del endpoint info debe reflejar el ConnectionString o un dummy/null. Recibido: {json}"
    );
    }
}
