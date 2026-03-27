using Microsoft.EntityFrameworkCore;
using ShiftService.Domain.Entities;

namespace ShiftService.Infrastructure.Persistence
{
    public class ShiftDbContext : DbContext
    {
        public ShiftDbContext(DbContextOptions<ShiftDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeUserMapping> EmployeeUserMappings { get; set; }
        
        public DbSet<Shift> Shifts  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.QrCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AccessAllowed)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false); // Nullable

                // Индекс для быстрого поиска по QR коду
                entity.HasIndex(e => e.QrCode)
                    .IsUnique(); // QR код должен быть уникальным

                // Индекс для поиска по имени
                entity.HasIndex(e => e.FullName);
            });

            // Настройка EmployeeUserMapping
            modelBuilder.Entity<EmployeeUserMapping>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.KeycloakUserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.KeycloakUsername)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.CreatedAt)
                    .IsRequired();

                entity.Property(m => m.LastLoginAt)
                    .IsRequired(false); // Nullable

                // Индексы для быстрого поиска
                entity.HasIndex(m => m.KeycloakUserId)
                    .IsUnique();

                entity.HasIndex(m => m.KeycloakUsername)
                    .IsUnique();

                // Связь с Employee (один к одному)
                entity.HasOne(m => m.Employee)
                    .WithOne(e => e.UserMapping)
                    .HasForeignKey<EmployeeUserMapping>(m => m.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении сотрудника удаляется и связь
                                                       
             // Настройка EmployeeUserMapping
             modelBuilder.Entity<Shift>(entity =>
                {
                    entity.HasKey(s => s.Id);

                    entity.Property(s => s.StartTime)
                        .IsRequired();

                    entity.Property(s => s.EndTime)
                        .IsRequired(false);

                    entity.HasOne(s => s.Employee)
                        .WithMany() 
                        .HasForeignKey(s => s.EmployeeId)
                        .OnDelete(DeleteBehavior.Cascade);

                    // Индексы для быстрых запросов
                    entity.HasIndex(s => s.EmployeeId);
                    entity.HasIndex(s => s.StartTime);
                    entity.HasIndex(s => s.EndTime);
                });
            });
        }
    }
}