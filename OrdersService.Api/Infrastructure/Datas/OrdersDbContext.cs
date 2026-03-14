using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Domain.Entities;

namespace OrdersService.Api.Infrastructure.Datas;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderNumber).IsRequired();
            entity.HasIndex(x => x.OrderNumber).IsUnique();

            entity.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasIndex(x => x.UserId);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .IsRequired();

            entity.Property(x => x.CompletedAt)
                .IsRequired(false);

            entity.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductId)
                .IsRequired();

            entity.Property(x => x.Quantity)
                .IsRequired();
        });
    }
}