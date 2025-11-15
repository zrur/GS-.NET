using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace HelpLink.Tests.Integration;

public class InstituicoesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public InstituicoesControllerTests(WebApplicationFactory<Program> factory)
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
                    options.UseInMemoryDatabase("InstituicoesTestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetInstituicoes_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/instituicoes?pageNumber=1&pageSize=10");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetInstituicoes_ReturnsValidJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/instituicoes?pageNumber=1&pageSize=10");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(content);
        Assert.NotEmpty(content);
        // Verifica estrutura de paginação (pode ser "items" ou "data")
        Assert.True(
            content.Contains("\"items\"", StringComparison.OrdinalIgnoreCase) ||
            content.Contains("\"data\"", StringComparison.OrdinalIgnoreCase),
            "Response should contain either 'items' or 'data' property"
        );
    }

    [Fact]
    public async Task GetInstituicao_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        int invalidId = 99999;

        // Act
        var response = await _client.GetAsync($"/api/v1/instituicoes/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetInstituicoes_WithPagination_ReturnsData()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/instituicoes?pageNumber=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
        Assert.Contains("pagenumber", content.ToLower());
        Assert.Contains("pagesize", content.ToLower());
    }
}
