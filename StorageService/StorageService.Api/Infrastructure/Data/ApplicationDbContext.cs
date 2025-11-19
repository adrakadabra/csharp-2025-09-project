using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    public static void SeedData(DbContext context)
    {
        context.Set<Product>().AddRange(FakeProductsData.Products);
        context.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(x => x.Id);
        });
    }
}
