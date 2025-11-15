using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace HelpLink.Tests.Integration;

public class DoacoesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DoacoesControllerTests(WebApplicationFactory<Program> factory)
    {
        var _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<HelpLinkDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HelpLinkDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DoacoesTestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetDoacoes_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/doacoes?pageNumber=1&pageSize=10");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetDoacoes_ReturnsValidJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/doacoes?pageNumber=1&pageSize=10");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(content);
        Assert.NotEmpty(content);
        // Verifica se é um JSON válido
        Assert.True(content.StartsWith("{") || content.StartsWith("["));
    }
}
