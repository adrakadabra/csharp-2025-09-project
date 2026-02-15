using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Database.Repositories;

internal sealed class PickerRepository(DatabaseContext databaseContext) : IPickerRepository
{
    public async Task<List<Picker>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await databaseContext.Pickers
            .Select( pickerEntity => pickerEntity.ToPicker())
            .ToListAsync(cancellationToken);
    }

    public async Task<Picker?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var picker = await databaseContext.Pickers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return picker?.ToPicker();
    }

    public async Task<Picker> CreateAsync(Picker picker, CancellationToken cancellationToken = default)
    {
        var pickerEntity = picker.ToPickerEntity();
        
        await databaseContext.Pickers.AddAsync(pickerEntity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return pickerEntity.ToPicker();
    }

    public async Task UpdateAsync(Picker picker, CancellationToken cancellationToken = default)
    {
        var entity = await databaseContext.Pickers.FindAsync([picker.Id], cancellationToken: cancellationToken);
        
        if(entity == null)
            throw new KeyNotFoundException($"Picker with id {picker.Id} not found in database");
        
        entity.FirstName = picker.FirstName;
        entity.LastName = picker.LastName;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";
        
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}