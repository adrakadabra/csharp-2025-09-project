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
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationItem> ReservationItems { get; set; }

    public static void SeedData(DbContext context)
    {
        if (context.Set<Product>().Any())
            return;

        var seed = JsonSeedDataLoader.LoadProductsSeedJson();

        var sections = JsonSeedDataLoader.BuildSections(seed);
        var categories = JsonSeedDataLoader.BuildCategories(seed);
        var manufacturers = JsonSeedDataLoader.BuildManufacturers(seed);
        var products = JsonSeedDataLoader.BuildProducts(seed);

        context.Set<Section>().AddRange(sections);
        context.Set<Category>().AddRange(categories);
        context.Set<Manufacturer>().AddRange(manufacturers);
        context.Set<Product>().AddRange(products);

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

            b.Property(x => x.Article)
                .IsRequired();

            b.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            b.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Manufacturer)
                .WithMany()
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(p => p.Section)
                 .WithMany(s => s.Products)
                 .HasForeignKey(p => p.SectionId)
                 .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(p => p.ReservationItems)
            .WithOne(s => s.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);


            b.HasIndex(x => x.Article)
                .IsUnique();

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

            b.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<Manufacturer>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            b.HasIndex(x => x.Name)
                .IsUnique();

            b.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<Section>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Code)
                .IsRequired();

            b.HasIndex(x => x.Code)
                .IsUnique();
        });

        modelBuilder.Entity<ReservationItem>(b =>
        {
            b.HasKey(x => x.Id);

            b.HasOne(p => p.Reservation)
            .WithMany(s => s.Items)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.OrderNumber)
                .IsUnique();
        });
    }
}