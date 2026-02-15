using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Section> Sections { get; set; }

    public static void SeedData(DbContext context)
    {
        context.Set<Section>().AddRange(FakeProductsData.Sections);
        context.Set<Product>().AddRange(FakeProductsData.Products);
        context.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Price)
                .HasPrecision(18, 2);

            b.Property(x => x.Quantity)
                .IsRequired();

            b.HasOne(x => x.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(x => x.CategoryId);

            b.HasOne(x => x.Manufacturer)
                .WithMany(m => m.Products)
                .HasForeignKey(x => x.ManufacturerId);

            b.HasOne(p => p.Section)
                 .WithMany(s => s.Products)
                 .HasForeignKey(p => p.SectionId)
                 .OnDelete(DeleteBehavior.Cascade);

            b.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<Category>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            b.HasQueryFilter(x => !x.IsDeleted);

            b.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Manufacturer>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            b.HasQueryFilter(x => !x.IsDeleted);

            b.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Section>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(32);

            b.HasIndex(x => x.Code)
                .IsUnique();
        });
    }
}
