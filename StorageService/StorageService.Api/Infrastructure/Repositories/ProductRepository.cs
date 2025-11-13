using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Data;

namespace StorageService.Api.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db) => _db = db;

    public async Task<Product> AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(Product[] items, long total)> GetPagedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _db.Products.OrderByDescending(p => p.CreatedAt).AsQueryable();
        var total = await query.LongCountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
        return (items, total);
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        product.SoftDelete();
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }
}
