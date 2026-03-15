using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using StorageService.Api.Application.DTOs;

namespace StorageService.Tests.Integration;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPaged_Should_Return_Paged_Products()
    {
        var res = await _client.GetAsync("/products?page=1&pageSize=5");

        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var paged = await res.Content.ReadFromJsonAsync<PagedResult<ProductDto>>();
        paged.Should().NotBeNull();
        paged!.Total.Should().BeGreaterThan(0);
        paged.Items.Should().NotBeNull();
        paged.Items.Should().HaveCount(5);
    }
}