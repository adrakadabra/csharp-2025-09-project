using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Domain;

namespace OrdersService.Api.Infrastructure.Persistence
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(o => o.Id);

                entity.Property(o => o.UserId)
                        .IsRequired();

                entity.Property(o => o.OrderStatus)
                        .IsRequired()
                        .HasConversion<int>();

                entity.Property(o => o.CreateAt)
                        .IsRequired();

                entity.Property(o => o.UpdateAt)
                        .IsRequired();

                entity.HasMany(o => o.Items)
                        .WithOne(i => i.Order)
                        .HasForeignKey(i => i.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");

                entity.HasKey(i => i.Id);

                entity.Property(i => i.ProductId)
                        .IsRequired();

                entity.Property(i => i.Quantity)
                        .IsRequired();
            });
        }
    }
}