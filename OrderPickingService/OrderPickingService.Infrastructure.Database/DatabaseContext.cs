using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Entities;
using OrderPickingService.Infrastructure.Database.Entities.Order;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Infrastructure.Database.Entities.PickingSession;

namespace OrderPickingService.Infrastructure.Database;

internal sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    { }
    
    public DbSet<PickerEntity> Pickers { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<PickingSessionEntity> PickingSessions { get; set; }
    public DbSet<PickedItemEntity> PickedItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var baseEntities = modelBuilder.Model.GetEntityTypes()
            .Where(entityType => entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
            .ToList();

        foreach (var entityType in baseEntities)
        {
            modelBuilder.Entity(entityType.ClrType, entity =>
            {
                entity.Property(nameof(BaseEntity.CreatedAt))
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");
                
                entity.Property(nameof(BaseEntity.CreatedBy))
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(nameof(BaseEntity.UpdatedAt));
            
                entity.Property(nameof(BaseEntity.UpdatedBy))
                    .HasMaxLength(100);
                
                entity.Property(nameof(BaseEntity.IsDeleted))
                    .IsRequired()
                    .HasDefaultValue(false);
                
                entity.Property(nameof(BaseEntity.DeletedAt));
            
                entity.Property(nameof(BaseEntity.DeletedBy))
                    .HasMaxLength(100);
                
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                    Expression.Constant(false)
                );
                entity.HasQueryFilter(Expression.Lambda(body, parameter));
            });
        }
        
        modelBuilder.Entity<PickerEntity>(
            pickerEntity =>
            {
                pickerEntity.HasKey(picker => picker.Id);
                
                pickerEntity.Property(picker => picker.FirstName)
                    .HasMaxLength(255); 
                
                pickerEntity.Property(picker => picker.LastName)
                    .HasMaxLength(255); 
            });
        
        modelBuilder.Entity<OrderEntity>(
            orderEntity =>
            {
                orderEntity.HasKey(order => order.Id);
                
                orderEntity.Property(order => order.ExternalId)
                    .IsRequired();
                
                orderEntity.Property(order => order.OrderStatus)
                    .IsRequired();
            });
        
        modelBuilder.Entity<OrderItemEntity>(
            orderItemEntity =>
            {
                orderItemEntity.HasKey(item => item.Id);
                
                orderItemEntity.Property(item => item.OrderId)
                    .IsRequired();
                
                orderItemEntity.Property(item => item.ProductExternalId)
                    .IsRequired();
                
                orderItemEntity.Property(item => item.ProductSku)
                    .HasMaxLength(255)
                    .IsRequired();
                
                orderItemEntity.Property(item => item.ProductName)
                    .HasMaxLength(255)
                    .IsRequired();
                
                orderItemEntity.Property(item => item.Quantity)
                    .IsRequired();
                
                orderItemEntity.Property(item => item.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();

                orderItemEntity.Property(item => item.Category)
                    .HasMaxLength(255)
                    .IsRequired();

                orderItemEntity.ToTable(table => table.HasCheckConstraint(
                    "CK_OrderItems_Quantity_Positive", 
                    "quantity > 0"));
                
                orderItemEntity.ToTable(table => table.HasCheckConstraint(
                    "CK_OrderItems_Price_NonNegative", 
                    "price >= 0"));
                
                orderItemEntity.HasIndex(item => item.OrderId);
            });
        
        modelBuilder.Entity<PickingSessionEntity>(
            pickingSessionEntity =>
            {
                pickingSessionEntity.HasKey(pickingSession => pickingSession.Id);
                
                pickingSessionEntity.Property(pickingSession => pickingSession.OrderId)
                    .IsRequired();  
                
                pickingSessionEntity.Property(pickingSession => pickingSession.PickerId)
                    .IsRequired();
                
                pickingSessionEntity.Property(pickingSession => pickingSession.StartedAt)
                    .IsRequired();
                
                pickingSessionEntity.Property(pickingSession => pickingSession.PickingStatus)
                    .IsRequired();

                pickingSessionEntity.Property(pickingSession => pickingSession.Notes)
                    .HasMaxLength(255);
                
                pickingSessionEntity.ToTable(table => table.HasCheckConstraint(
                    "CK_PickingSessions_FinishedAt_GreaterThanOrEqualTo_StartedAt", 
                    "finished_at >= started_at"));
                
                pickingSessionEntity.HasIndex(pickingSession => pickingSession.OrderId);
                
                pickingSessionEntity.HasIndex(pickingSession => pickingSession.PickerId);
            });

        modelBuilder.Entity<PickedItemEntity>(
            pickedItemEntity =>
            {
                pickedItemEntity.HasKey(pickedItem => pickedItem.Id);

                pickedItemEntity.Property(pickedItem => pickedItem.PickingSessionId)
                    .IsRequired();
                
                pickedItemEntity.Property(pickedItem => pickedItem.OrderItemId)
                    .IsRequired();  
                
                pickedItemEntity.Property(pickedItem => pickedItem.PickedAt)
                    .IsRequired() 
                    .HasDefaultValueSql("NOW()"); 
                
                pickedItemEntity.Property(pickedItem => pickedItem.Note)
                    .HasMaxLength(255);

                pickedItemEntity.HasIndex(pickedItem => pickedItem.PickingSessionId);
                
                pickedItemEntity.HasIndex(pickedItem => pickedItem.OrderItemId);
            });
    }
}
//Сгенерировать миграцию (AddOrderTables - название миграции)
//dotnet ef migrations add AddOrderTables --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

//Обновить БД
//dotnet ef database update --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

// Обновить БД до миграции (AddOrderTables - название миграции)
// dotnet ef database update AddOrderTables --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

//Удалить последнюю миграцию
//dotnet ef migrations remove --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext