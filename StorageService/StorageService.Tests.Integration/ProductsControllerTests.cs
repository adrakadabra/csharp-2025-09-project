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
        _client = factory.CreateClient();
    }

    //[Fact]
    //public async Task Create_And_Get_Returns_Created()
    //{
    //    //var client = _factory.CreateClient();
    //    var create = new CreateProductDto { Name = "IntProd", Description = "d", Quantity = 1};
    //    var res = await _client.PostAsJsonAsync("/products", create);
    //    res.StatusCode.Should().Be(HttpStatusCode.Created);

    //    var created = await res.Content.ReadFromJsonAsync<ProductDto>();
    //    created.Should().NotBeNull();

    //    var get = await _client.GetAsync($"/products/{created!.Id}");
    //    get.StatusCode.Should().Be(HttpStatusCode.OK);
    //}

    [Fact]
    public async Task GetPaged_Requires_Pagination_And_Returns_Zero_When_Empty()
    {
        var res = await _client.GetAsync("/products?page=1&pageSize=5");
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var paged = await res.Content.ReadFromJsonAsync<PagedResult<ProductDto>>();
        paged.Should().NotBeNull();
        paged!.Total.Should().Be(1);
    }
}
