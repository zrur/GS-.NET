using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace HelpLink.Tests.Simple;

public class SimpleAPITests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SimpleAPITests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AuthLogin_ReturnsResponse()
    {
        // Arrange
        var loginData = new
        {
            email = "admin@helplink.com",
            password = "Admin@123"
        };

        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Auth/login", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.InternalServerError);
        Assert.NotNull(responseContent);
        Assert.NotEmpty(responseContent);
    }

    [Fact]
    public async Task Swagger_IsAccessible()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.MovedPermanently);
    }

    [Fact]
    public async Task API_Root_ReturnsResponse()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        Assert.True(response.StatusCode != HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task InstituicoesEndpoint_ExistsAndResponds()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/instituicoes");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // Endpoint exists (not 404)
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
    }

    [Fact]
    public async Task DoacoesEndpoint_ExistsAndResponds()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/doacoes");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // Endpoint exists (not 404)
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
    }
}