using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Moq;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Services;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Tests.Unit;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Call_Repository_And_Return_Dto()
    {
        var repoMock = new Mock<IProductRepository>();
        var catServiceMock = new Mock<ICategoryService>();
        var manuServiceMock = new Mock<IManufacturerService>();
        var sectionServiceMock = new Mock<ISectionService>();
        var busControlMock = new Mock<IBusControl>();
        var configurationMock = new Mock<IConfiguration>();
        var mockConfigurationSection = new Mock<IConfigurationSection>();


        mockConfigurationSection.Setup(s => s.Value).Returns("http://someservice:81");
        configurationMock.Setup(c => c.GetSection("RabbitMqConfiguration"))
          .Returns(mockConfigurationSection.Object);
        repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) =>
            {
                p.Manufacturer = new Manufacturer();
                p.Category = new Category();
                p.Section = new Section();
                return p;
            });

        sectionServiceMock.Setup(s => s.GetByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new SectionDto { Code = "M3"});
        catServiceMock.Setup(s => s.GetOrCreateAsync(It.IsAny<string>()))
            .ReturnsAsync(new CategoryDto { Name = "test", Description="test" });
        manuServiceMock.Setup(s => s.GetOrCreateAsync(It.IsAny<string>()))
            .ReturnsAsync(new ManufacturerDto { Name = "test", Country = "test" });

        var service = new ProductService(repoMock.Object, catServiceMock.Object,
            manuServiceMock.Object, sectionServiceMock.Object, busControlMock.Object, configurationMock.Object);

        var dto = new CreateProductDto("Test", "1274y8241", "Desc", 5, 100, "test", "test", "M3");

        var result = await service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Returns_False_When_NotFound()
    {
        var repoMock = new Mock<IProductRepository>();
        var catServiceMock = new Mock<ICategoryService>();
        var manuServiceMock = new Mock<IManufacturerService>();
        var sectionServiceMock = new Mock<ISectionService>();
        var busControlMock = new Mock<IBusControl>();
        var configurationMock = new Mock<IConfiguration>();
        var mockConfigurationSection = new Mock<IConfigurationSection>();

        mockConfigurationSection.Setup(s => s.Value).Returns("http://someservice:81");
        configurationMock.Setup(c => c.GetSection("RabbitMqConfiguration"))
          .Returns(mockConfigurationSection.Object);
        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?)null);

        var service = new ProductService(repoMock.Object, catServiceMock.Object,
            manuServiceMock.Object, sectionServiceMock.Object, busControlMock.Object, configurationMock.Object);
        var ok = await service.UpdateAsync(Guid.NewGuid(), new UpdateProductDto { Name = "X", Quantity = 1, Price = 1m });

        ok.Should().BeFalse();
    }
}
