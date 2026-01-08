using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingService.Infrastructure.Database.Abstractions;
using OrderPickingService.Infrastructure.Database.Dtos;

namespace OrderPickingService.Infrastructure.Database;

internal sealed class DatabaseContext : DbContext, IDataBaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    { }
    
    public DbSet<Picker> Pickers { get; set; }
    
    
    // public List<Picker> GetPickers()
    // {
    //     return Pickers.ToList();
    // }
    
    // public int Get()
    // {
    //     return Database
    //         .SqlQueryRaw<int>("SELECT 1")
    //         .AsEnumerable()
    //         .FirstOrDefault();
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var baseEntities = modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsSubclassOf(typeof(BaseEntity)))
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
                
                // var parameter = Expression.Parameter(entityType.ClrType, "e");
                // var body = Expression.Equal(
                //     Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                //     Expression.Constant(false)
                // );
                // entity.HasQueryFilter(Expression.Lambda(body, parameter));
            });
        }

        
        modelBuilder.Entity<Picker>(
            picker =>
            {
                picker.HasKey(p => p.Id);
                picker.Property(p => p.FirstName).HasMaxLength(255); 
                picker.Property(p => p.LastName).HasMaxLength(255); 
            });
    }

}
//Сгенерировать миграцию (AddDefaultColumns - название миграции)
//dotnet ef migrations add AddDefaultColumns --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

//Обновить БД
//dotnet ef database update --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

// Обновить БД до миграции (AddDefaultColumns - название миграции)
// dotnet ef database update AddDefaultColumns --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext

//Удалить последнюю миграцию
//dotnet ef migrations remove --startup-project ../OrderPickingService.Api --project ../OrderPickingService.Infrastructure.Database --context DatabaseContext