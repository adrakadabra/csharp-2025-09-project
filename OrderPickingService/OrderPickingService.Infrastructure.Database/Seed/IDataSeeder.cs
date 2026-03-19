using Microsoft.EntityFrameworkCore;
namespace OrderPickingService.Infrastructure.Database.Seed;

public interface IDataSeeder
{
    void Seed(ModelBuilder modelBuilder);
}