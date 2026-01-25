using Moq;
using FluentAssertions;
using StorageService.Api.Application.Services;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Tests.Unit;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Call_Repository_And_Return_Dto()
    {
        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p);

        var service = new ProductService(repoMock.Object);
        var dto = new CreateProductDto { Name = "Test", Description = "Desc", Quantity = 5};

        var result = await service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Returns_False_When_NotFound()
    {
        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        var service = new ProductService(repoMock.Object);
        var ok = await service.UpdateAsync(Guid.NewGuid(), new UpdateProductDto { Name = "X", Quantity = 1, Price = 1m });

        ok.Should().BeFalse();
    }
}
