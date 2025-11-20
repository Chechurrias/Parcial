using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProgramTests(WebApplicationFactory<Program> factory)
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
        // Arrange: Crea payload tipo "Demo,TestItem,2,10.0"
        var content = new StringContent("Demo,TestItem,2,10", System.Text.Encoding.UTF8, "text/plain");

        // Act
        var response = await _client.PostAsync("/orders", content);

        // Assert
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
        Assert.Contains("Server=localhost", json); // O ajusta el string según tu config
    }
}
