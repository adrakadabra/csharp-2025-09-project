using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Repositories;
using StorageService.Api.Domain.Entities;

namespace StorageService.Tests.Unit;

public class ProductRepositoryTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task AddAndGetById_Works()
    {
        var ctx = CreateContext();
        var repo = new ProductRepository(ctx);

        var section = await ctx.Sections.AddAsync(new Section { Id = Guid.NewGuid(), Code = "M3", Description = "desctiption" });
        var manuf = await ctx.Manufacturers.AddAsync(new Manufacturer { Id = Guid.NewGuid(), Country = "Russia", Name = "Autovaz" });
        var category = await ctx.Categories.AddAsync(new Category { Id = Guid.NewGuid(), Name = "test", Description = "test" });

        var p = new Product
        {
            Id = Guid.NewGuid(),
            Name = "R1",
            Quantity = 2,
            Price = 3m,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "user",
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = "user",
            Article = "asd",
            CategoryId = category.Entity.Id,
            ManufacturerId = manuf.Entity.Id,
            SectionId = section.Entity.Id,
        };
        await repo.AddAsync(p);

        var fetched = await repo.GetByIdAsync(p.Id);

        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("R1");
    }

    [Fact]
    public async Task SoftDelete_Hides_From_GetPaged()
    {
        var ctx = CreateContext();
        var repo = new ProductRepository(ctx);
        var section = await ctx.Sections.AddAsync(new Section { Id = Guid.NewGuid(), Code = "M3", Description = "desctiption" });
        var manuf = await ctx.Manufacturers.AddAsync(new Manufacturer { Id = Guid.NewGuid(), Country = "Russia", Name = "Autovaz" });
        var category = await ctx.Categories.AddAsync(new Category { Id = Guid.NewGuid(), Name = "test", Description = "test" });

        var p = new Product
        {
            Id = Guid.NewGuid(),
            Name = "ToDelete",
            Quantity = 2,
            Price = 3m,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "user",
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = "user",
            Article = "asd",
            CategoryId = category.Entity.Id,
            ManufacturerId = manuf.Entity.Id,
            SectionId = section.Entity.Id
        };
        await repo.AddAsync(p);
        await repo.DeleteAsync(p);

        var (items, total) = await repo.GetPagedAsync(1, 10);

        total.Should().Be(0);
    }
}
